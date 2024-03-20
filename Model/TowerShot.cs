using System;

public class TowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;
    private int _damage = 4;

    public bool IsComplete => Info.ProgressRatio > 1;

    public TowerShotInfo Info { get; } = new();

    public Enemy Target { get; }

    public TowerShot()
    {
        _creationTime = DateTime.UtcNow;
    }

    public TowerShot(Enemy target)
    {
        _creationTime = DateTime.UtcNow;
        Target = target;
        Info.TargetId = target.Info.Id;
    }

    public void DoLoop()
    {
        Info.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
        if (IsComplete)
        {
            Target.TakeDamage(_damage);
        }
    }
}