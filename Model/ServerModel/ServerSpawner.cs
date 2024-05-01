using System;
using System.Timers;

public class ServerSpawner
{
    private Timer _spawnTimer;

    public Spawner Spawner { get; } = new();

    public ServerSpawner(EnemyInfo info)
    {
        _spawnTimer = new Timer(info.SpawnTimerMs);
        _spawnTimer.AutoReset = true;
        _spawnTimer.Elapsed += (s, a) => IncrementQueue();
        Spawner.UnitType = info.Type;
        Spawner.Max = info.SpawnMax;
        Spawner.CurrentValue = 0;
        _spawnTimer.Start();
    }

    private object _queueLock = new();
    public void IncrementQueue()
    {
        lock (_queueLock)
        {
            if (Spawner.CurrentValue < Spawner.Max)
            {
                Spawner.CurrentValue++;
                if (Spawner.CurrentValue == Spawner.Max)
                {
                    _spawnTimer.Stop();
                }
            }
        }
    }

    public bool DecrementQueue()
    {
        lock (_queueLock)
        {
            if (Spawner.CurrentValue > 0)
            {
                Spawner.CurrentValue--;
                _spawnTimer.Start();
                return true;
            }
            return false;
        }
    }
}