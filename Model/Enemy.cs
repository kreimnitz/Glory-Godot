using System;

public class Enemy
{
    private int _durationMs = 5000;
    private DateTime _creationTime;

    public EnemyInfo Info { get; set; } = new();

    public Enemy()
    {
        _creationTime = DateTime.UtcNow;
    }

    public void TakeDamage(int damage)
    {
        Info.HpCurrent -= damage;
    }

    public bool IsDead()
    {
        return Info.ProgressRatio > 1 || Info.HpCurrent <= 0;
    }

    public void DoLoop()
    {
        Info.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }
}