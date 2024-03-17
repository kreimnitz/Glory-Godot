using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tower
{
    private int _cooldownMs = 500;
    private DateTime _lastShotTime = DateTime.MinValue;

    public TowerShot Shoot(Enemy target)
    {
        return new TowerShot(target);
    }

    public TowerShot CheckForNewShot(IEnumerable<Enemy> enemies)
    {
        if (!enemies.Any() || (DateTime.UtcNow - _lastShotTime).TotalMilliseconds < _cooldownMs)
        {
            return null;
        }

        var closestEnemy = enemies.OrderByDescending(e => e.Info.ProgressRatio).First();
        if (closestEnemy.Info.ProgressRatio < .5)
        {
            return null;
        }

        _lastShotTime = DateTime.UtcNow;
        return Shoot(closestEnemy);
    }
}