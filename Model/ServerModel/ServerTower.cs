using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class ServerTower
{
    private readonly int[] _damageLevels = { 4, 6, 8, 10 };
    private int DamageLevel { get; set; } = 0;
    
    private readonly int[] _cooldownLevels = { 500, 400, 300, 200 };
    public int CooldownLevel { get; set; } = 0;

    private DateTime _lastShotTime = DateTime.MinValue;

    public Vector2 Position { get; private set; } = new(0, 150);

    public ServerTowerShot Shoot(ServerEnemy target)
    {
        return new ServerTowerShot(Position, target, _damageLevels[DamageLevel]);
    }

    public ServerTowerShot CheckForNewShot(IEnumerable<ServerEnemy> enemies)
    {
        var elapsedMs = (DateTime.UtcNow - _lastShotTime).TotalMilliseconds;
        if (!enemies.Any() || elapsedMs < _cooldownLevels[CooldownLevel])
        {
            return null;
        }

        var closestEnemy = enemies.OrderByDescending(e => e.Enemy.Progress).First();
        if (closestEnemy.Enemy.Progress < 150)
        {
            return null;
        }

        _lastShotTime = DateTime.UtcNow;
        return Shoot(closestEnemy);
    }
}