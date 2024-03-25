using System;
using System.Collections.Generic;
using System.Linq;

public class Tower
{
    private readonly int[] _damageLevels = { 4, 6, 8, 10 };
    private int DamageLevel { get; set; } = 0;
    
    private readonly int[] _cooldownLevels = { 500, 400, 300, 200 };
    public int CooldownLevel { get; set; } = 0;

    private DateTime _lastShotTime = DateTime.MinValue;

    public ServerTowerShot Shoot(ServerEnemy target)
    {
        return new ServerTowerShot(target, _damageLevels[DamageLevel]);
    }

    public ServerTowerShot CheckForNewShot(IEnumerable<ServerEnemy> enemies)
    {
        var elapsedMs = (DateTime.UtcNow - _lastShotTime).TotalMilliseconds;
        if (!enemies.Any() || elapsedMs < _cooldownLevels[CooldownLevel])
        {
            return null;
        }

        var closestEnemy = enemies.OrderByDescending(e => e.ProgressRatio).First();
        if (closestEnemy.ProgressRatio < .5)
        {
            return null;
        }

        _lastShotTime = DateTime.UtcNow;
        return Shoot(closestEnemy);
    }
}