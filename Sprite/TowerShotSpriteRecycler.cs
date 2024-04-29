using Godot;
using System;
using System.Collections.Generic;

public class TowerShotSpriteManager
{
    private Dictionary<Guid, TowerShotSprite> _idToTowerShotSprite = new();
    private Queue<TowerShotSprite> _hiddenShots = new Queue<TowerShotSprite>();
    public TowerShotSprite CreateSprite(TowerShot towerShot, Node parent)
    {
        if (_hiddenShots.TryDequeue(out TowerShotSprite toRecycle))
        {
            toRecycle.Model = towerShot;
            toRecycle.Show();
            _idToTowerShotSprite[towerShot.Id] = toRecycle;
            return toRecycle;
        }

        var shotSprite = TowerShotSprite.CreateTowerShotSprite(towerShot);
        shotSprite.Hidden += () => _hiddenShots.Enqueue(shotSprite);
        _idToTowerShotSprite[towerShot.Id] = shotSprite;
        parent.AddChild(shotSprite);
        return shotSprite;
    }

    public void RecycleSprite(Guid id)
    {
        if (_idToTowerShotSprite.TryGetValue(id, out TowerShotSprite towerShotSprite))
        {
            towerShotSprite.Model = null;
            towerShotSprite.Hide();
            _idToTowerShotSprite.Remove(id);
        }
    }
}