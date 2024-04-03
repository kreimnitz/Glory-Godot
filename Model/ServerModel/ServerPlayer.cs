using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

public class ServerPlayer : Player
{
    public const int StartingGlory = 0;
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDurationMs = 10000;
    public const int IncomeTimerMs = 1000;

    private ActionQueue _actionQueue = new();
    private Timer _incomeTimer = new(IncomeTimerMs);

    public int PlayerNumber { get; }

    public DelayedActionQueue InProgressQueue { get; } = new();

    public Tower Tower { get; } = new();

    private IEnumerable<ServerEnemy> ServerEnemies => Enemies.Cast<ServerEnemy>();
    private IEnumerable<ServerTowerShot> ServerTowerShots => TowerShots.Cast<ServerTowerShot>();

    public ServerPlayer OtherPlayer { get; set; }

    public ServerPlayer()
    {
        Glory = StartingGlory;
        FollowerCount = StartingFollowerCount;
        _incomeTimer.Elapsed += (s, a) => _actionQueue.Add(ApplyIncome);
        _incomeTimer.Start();
    }

    public ServerPlayer(int playerNumber) : this()
    {
        PlayerNumber = playerNumber;
    }

    public void DoLoop()
    {
        _actionQueue.ExecuteActions();
        InProgressQueue.ApplyNextActionIfReady();
        TaskQueue = InProgressQueue.Actions.Select(da => da.ToProgressItem()).ToList();
        foreach (var enemy in ServerEnemies)
        {
            enemy.DoLoop();
        }
        foreach (var towerShot in ServerTowerShots)
        {
            towerShot.DoLoop();
        }

        CheckForNewShot();
        CheckLifetimes();
    }

    private void ApplyIncome()
    {
        Glory += FollowerCount * IncomePerFollower;
    }

    public void HandleAddFollowerRequest()
    {
        if (Glory < FollowerCost)
        {
            return;
        }

        Glory -= FollowerCost;
        var delayedAction = new DelayedAction(() => FollowerCount++, FollowerTrainDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleAddFireTempleRequest()
    {
        if (Glory < ServerTemple.Cost)
        {
            return;
        }

        Glory -= ServerTemple.Cost;
        var delayedAction = new DelayedAction(() => FireTemple.IsActive = true, ServerTemple.CreateDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleAddVentSpawnerRequest()
    {
        var fireTemple = (ServerTemple)FireTemple;
        if (!fireTemple.IsActive || Glory < Spawners.VentSpawnerCost)
        {
            return;
        }

        Glory -= Spawners.VentSpawnerCost;
        var ventSpawner = Spawners.CreateVentSpawner(fireTemple);
        var delayedAction = new DelayedAction(() => ventSpawner.Activate(), Spawners.VentCreateDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleSpawnFireImpRequest()
    {
        var impSpawner = ((ServerTemple)FireTemple).GetSpawnerForType(EnemyType.FireImp);
        if (Glory < EnemyUtilites.FireImpCost || !impSpawner.DecrementQueue())
        {
            return;
        }

        Glory -= EnemyUtilites.FireImpCost;
        var imp = EnemyUtilites.CreateFireImp();
        AddEnemy(imp);
    }

    public void AddEnemy(ServerEnemy enemy)
    {
        _actionQueue.Add(() => Enemies.Add(enemy));
    }

    public void CheckForNewShot()
    {
        var shot = Tower.CheckForNewShot(ServerEnemies);
        if (shot is not null)
        {
            TowerShots.Add(shot);
        }
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < Enemies.Count)
        {
            ServerEnemy enemy = (ServerEnemy)Enemies[0];
            if (enemy.IsDead())
            {
                Enemies.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }

        index = 0;
        while (index < TowerShots.Count)
        {
            if (((ServerTowerShot)TowerShots[0]).IsComplete)
            {
                TowerShots.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }
    }
}