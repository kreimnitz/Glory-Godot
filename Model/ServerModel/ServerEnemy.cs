using System;

public class ServerEnemy : Enemy
{
    private int _durationMs = 5000;
    private DateTime _creationTime;

    public ServerEnemy()
    {
        _creationTime = DateTime.UtcNow;
    }

    public ServerEnemy(int hp) : this()
    {
        HpMax = hp;
        HpCurrent = hp;
    }

    public void TakeDamage(int damage)
    {
        HpCurrent -= damage;
    }

    public bool IsDead()
    {
        return ProgressRatio > 1 || HpCurrent <= 0;
    }

    public void DoLoop()
    {
        ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }
}