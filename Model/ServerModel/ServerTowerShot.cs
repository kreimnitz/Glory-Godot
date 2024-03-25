using System;

public class ServerTowerShot : TowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;
    private int _damage = 4;

    public bool IsComplete => ProgressRatio > 1;

    public ServerEnemy Target { get; }

    public ServerTowerShot()
    {
        _creationTime = DateTime.UtcNow;
    }

    public ServerTowerShot(ServerEnemy target, int damage)
    {
        _creationTime = DateTime.UtcNow;
        _damage = damage;
        Target = target;
        TargetId = target.Id;
    }

    public void DoLoop()
    {
        ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
        if (IsComplete)
        {
            Target.TakeDamage(_damage);
        }
    }
}