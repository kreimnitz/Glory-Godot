public class ServerPlayer
{
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
    public ServerSummonGate ServerSummonGate { get; }

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

        Player.Glory = Player.StartingGlory;
        _incomeTimer.Elapsed += (s, a) => _actionQueue.Add(ApplyIncome);
        _incomeTimer.Start();
        PlayerNumber = playerNumber;

        var mainTemple = new ServerTemple(this, 0);
        mainTemple.Temple.FollowerCount = Player.StartingFollowerCount;
        mainTemple.Temple.IsActive = true;
        ServerTemples.Add(mainTemple);

        EnemyPath = new EnemyPath(EnemyPath.CreateWindingPathCurve());

        ServerSummonGate = new(this);
        ServerSummonGate.CreateSpawner(Enemies.WarriorInfo);
    }

    public void DoLoop()
    {
        _actionQueue.ExecuteActions();
        InProgressQueue.ApplyNextActionIfReady();
        Player.TaskQueue = InProgressQueue.ToProgressItemList();
        ServerSummonGate.DoLoop();
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

    public void BuildTemple(int position)
    {
        var newTemple = new ServerTemple(this, position);
        ServerTemples.Add(newTemple);
        newTemple.QueueBuild();
    }

    private void ApplyIncome()
    {
        Player.Glory += Player.TotalFollowerCount * Follower.IncomePerFollower;
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

    public void TakeDamage(int damage)
    {
        Player.HpCurrent -= damage;
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < _serverEnemies.Count)
        {
            var enemy = _serverEnemies[0];
            if (enemy.IsDead)
            {
                if (enemy.ReachedEndOfPath)
                {
                    TakeDamage(1);
                }
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