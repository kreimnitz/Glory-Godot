using System;
using System.Timers;

public class ServerSpawner
{
    private Timer _spawnTimer;

    public Spawner Spawner { get; } = new();

    public ServerSpawner()
    {
    }

    public ServerSpawner(
        int timerMs,
        UnitType unitType)
    {
        _spawnTimer = new Timer(timerMs);
        _spawnTimer.AutoReset = true;
        _spawnTimer.Elapsed += (s, a) => IncrementQueue();
        Spawner.UnitType = unitType;
        Spawner.Max = 5;
        Spawner.CurrentValue = 0;
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

    public void Activate()
    {
        _spawnTimer.Start();
    }
}