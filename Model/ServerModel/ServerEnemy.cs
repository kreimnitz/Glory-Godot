using System;

public class ServerEnemy
{
    private int _durationMs = 5000;
    private DateTime _creationTime;

    public Enemy Enemy { get; private set; }

    public ServerEnemy()
    {
        _creationTime = DateTime.UtcNow;
    }

    public ServerEnemy(int hp) : this()
    {
        Enemy.HpMax = hp;
        Enemy.HpCurrent = hp;
    }

    public void TakeDamage(int damage)
    {
        Enemy.HpCurrent -= damage;
    }

    public bool IsDead()
    {
        return Enemy.ProgressRatio > 1 || Enemy.HpCurrent <= 0;
    }

    public void DoLoop()
    {
        Enemy.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }
}