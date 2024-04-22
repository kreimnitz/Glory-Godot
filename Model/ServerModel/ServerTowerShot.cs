using System;

public class ServerTowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;
    private int _damage = 4;

    public bool IsComplete => TowerShot.ProgressRatio > 1;

    public ServerEnemy Target { get; }

    public TowerShot TowerShot { get; private set; } = new();

    public ServerTowerShot(ServerEnemy target, int damage)
    {
        _creationTime = DateTime.UtcNow;
        _damage = damage;
        Target = target;
        TowerShot.TargetId = target.Enemy.Id;
    }

    public void DoLoop()
    {
        TowerShot.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
        if (IsComplete)
        {
            Target.TakeDamage(_damage);
        }
    }
}