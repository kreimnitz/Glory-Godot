using System;
using System.Timers;

public class ServerSpawner : Spawner
{
    private Timer _spawnTimer;

    public ServerSpawner()
    {
    }

    public ServerSpawner(
        int timerMs,
        EnemyType enemyType)
    {
        _spawnTimer = new Timer(timerMs);
        _spawnTimer.AutoReset = true;
        _spawnTimer.Elapsed += (s, a) => IncrementQueue();
        EnemyType = enemyType;
    }

    private object _queueLock = new();
    public void IncrementQueue()
    {
        lock (_queueLock)
        {
            if (CurrentValue < Max)
            {
                CurrentValue++;
                if (CurrentValue == Max)
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
            if (CurrentValue > 0)
            {
                CurrentValue--;
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