using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Godot;

public class ServerPlayer
{
    public const int StartingGlory = 0;
    public const int IncomeTimerMs = 1000;

    public Player Player { get; } = new();
    private ActionQueue _actionQueue = new();
    private System.Timers.Timer _incomeTimer = new(IncomeTimerMs);

    public int PlayerNumber { get; }

    public DelayedActionQueue InProgressQueue { get; } = new();

    public ServerTower Tower { get; } = new();

    private SyncedList<ServerEnemy, Enemy> _serverEnemies;
    private SyncedList<ServerTowerShot, TowerShot> _serverTowerShots;
    public SyncedList<ServerTemple, Temple> ServerTemples { get; }

    public ServerPlayer Opponent { get; set; }

    public EnemyPath EnemyPath { get; set; }

    public ServerPlayer()
    {
    }

    public ServerPlayer(int playerNumber)
    {
        _serverEnemies = new(Player.Enemies, (serverEnemy) => serverEnemy.Enemy);
        _serverTowerShots = new(Player.TowerShots, (serverTowerShot) => serverTowerShot.TowerShot);
        ServerTemples = new(Player.Temples, (serverTemple) => serverTemple.Temple);


        Player.Glory = StartingGlory;
        _incomeTimer.Elapsed += (s, a) => _actionQueue.Add(ApplyIncome);
        _incomeTimer.Start();
        PlayerNumber = playerNumber;

        for (int i = 0; i < Player.TempleCount; i++)
        {
            ServerTemples.Add(new ServerTemple(this));
        }
        ServerTemples[0].Temple.IsActive = true;
        ServerTemples[0].Temple.FollowerCount = 10;

        EnemyPath = new EnemyPath(EnemyPath.CreateWindingPathCurve());
    }

    public void DoLoop()
    {
        _actionQueue.ExecuteActions();
        InProgressQueue.ApplyNextActionIfReady();
        Player.TaskQueue = InProgressQueue.ToProgressItemList();
        foreach (var temple in ServerTemples)
        {
            temple.DoLoop();
        }
        foreach (var enemy in _serverEnemies)
        {
            enemy.DoLoop();
        }
        foreach (var towerShot in _serverTowerShots)
        {
            towerShot.DoLoop();
        }

        CheckForNewShot();
        CheckLifetimes();
    }

    public void UnlockFlameImp()
    {
        Player.Tech.FireTech |= FireTech.FlameImp;
        foreach (var temple in ServerTemples)
        {
            if (temple.Temple.Element == Element.Fire)
            {
                var spawner = Spawners.CreateFireImpSpawner();
                temple.ServerSpawners.Add(spawner);
                spawner.Activate();
            }
        }
    }

    private void ApplyIncome()
    {
        for (int i = 0; i < Player.TempleCount; i++)
        {
            Player.Glory += Player.Temples[i].FollowerCount * ServerTemple.IncomePerFollower;
        }
    }

    public void AddEnemy(ServerEnemy enemy)
    {
        _actionQueue.Add(() => _serverEnemies.Add(enemy));
    }

    public void CheckForNewShot()
    {
        var shot = Tower.CheckForNewShot(_serverEnemies);
        if (shot is not null)
        {
            _serverTowerShots.Add(shot);
        }
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < _serverEnemies.Count)
        {
            var enemy = _serverEnemies[0];
            if (enemy.IsDead())
            {
                _serverEnemies.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }

        index = 0;
        while (index < _serverTowerShots.Count)
        {
            if (_serverTowerShots[0].IsComplete)
            {
                _serverTowerShots.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }
    }
}