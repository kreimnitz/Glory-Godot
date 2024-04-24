using Godot;

public class ServerTowerShot
{
    private double _speedPxPerS = 600.0;
    private double _speedPxPerLoop;
    private int _damage = 4;

    public ServerEnemy Target { get; }

    public TowerShot TowerShot { get; private set; } = new();

    public bool IsComplete { get; private set; }

    public ServerTowerShot(Vector2 initialPosition, ServerEnemy target, int damage)
    {
        _speedPxPerLoop = _speedPxPerS * ServerGameState.LoopRateMs / 1000;
        _damage = damage;
        Target = target;
        TowerShot.TargetId = target.Enemy.Id;
        TowerShot.PositionX = initialPosition.X;
        TowerShot.PositionY = initialPosition.Y;
    }

    public void DoLoop()
    {
        if (IsComplete)
        {
            return;
        }

        var pos = new Vector2(TowerShot.PositionX, TowerShot.PositionY);
        var diff = Target.Position - pos;
        var diffLength = diff.Length();
        if (diffLength < _speedPxPerLoop)
        {
            pos = Target.Position;
            Target.TakeDamage(_damage);
            IsComplete = true;
        }
        else
        {
            var ratio = _speedPxPerLoop / diffLength;
            pos += diff * (float)ratio;
        }

        TowerShot.PositionX = pos.X;
        TowerShot.PositionY = pos.Y;
    }
}