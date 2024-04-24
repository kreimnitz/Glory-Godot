using Godot;

public class ServerEnemy
{
    private double _speedPxPerS = 200.0;
    private double _speedPxPerLoop;
    private EnemyPath _path;

    public Enemy Enemy { get; private set; } = new();
    public Vector2 Position { get; private set; } = new();

    public ServerEnemy(int hp, EnemyPath path)
    {
        _speedPxPerLoop = _speedPxPerS * ServerGameState.LoopRateMs / 1000;
        _path = path;
        Enemy.HpMax = hp;
        Enemy.HpCurrent = hp;
        Enemy.Progress = 0;
        Position = path.GetPosition(0);
    }

    public void TakeDamage(int damage)
    {
        Enemy.HpCurrent -= damage;
    }

    public bool IsDead()
    {
        return Enemy.Progress > _path.Length || Enemy.HpCurrent <= 0;
    }

    public void DoLoop()
    {
        if (IsDead())
        {
            return;
        }
        Enemy.Progress += (float)_speedPxPerLoop;
        Position = _path.GetPosition(Enemy.Progress);
    }
}