using System;
using System.Timers;

public class ServerSpawner : Spawner
{
    private Timer _spawnTimer;
    private ServerTemple _temple;

    public ServerSpawner()
    {
    }

    public ServerSpawner(
        ServerTemple temple,
        int timerMs,
        EnemyType enemyType)
    {
        _temple = temple;
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
            if (QueueCount < QueueMax)
            {
                QueueCount++;
                if (QueueCount == QueueMax)
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
            if (QueueCount > 0)
            {
                QueueCount--;
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