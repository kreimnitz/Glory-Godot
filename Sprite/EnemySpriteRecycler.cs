using Godot;
using System;
using System.Collections.Generic;

public class EnemySpriteManager
{
    private Queue<EnemySprite> _hiddenEnemies = new Queue<EnemySprite>();
    private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();
    public EnemySprite CreateSprite(Enemy model, Path2D path)
    {
        if (_hiddenEnemies.TryDequeue(out EnemySprite toRecycle))
        {
            toRecycle.Reset(model);
            _enemyIdToSprite[model.Id] = toRecycle;
            return toRecycle;
        }

        var enemySprite = EnemySprite.CreateEnemy(model, path);
        enemySprite.Hidden += () => _hiddenEnemies.Enqueue(enemySprite);
        _enemyIdToSprite[model.Id] = enemySprite;
        return enemySprite;
    }

    public void RecycleSprite(Guid id)
    {
        if (_enemyIdToSprite.TryGetValue(id, out EnemySprite enemySprite))
        {
            enemySprite.Kill();
            _enemyIdToSprite.Remove(id);
        }
    }
}