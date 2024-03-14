using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TowerSprite : Sprite2D
{
	private PackedScene _towerShotScene;
	private Dictionary<Guid, TowerShotSprite> _idToTowerShotSprite = new();

	private int _cooldownMs = 500;

	private Queue<TowerShotSprite> _hiddenShots = new Queue<TowerShotSprite>();
	private TowerShotSprite CreateTowerShotSprite(TowerShot towerShot, EnemySprite targetSprite)
	{
		if (_hiddenShots.TryDequeue(out TowerShotSprite toRecycle))
		{
			toRecycle.Reset(towerShot);
			_idToTowerShotSprite[towerShot.Id] = toRecycle;
			return toRecycle;
		}

		var shotSprite = TowerShotSprite.CreateTowerShotSprite(towerShot, this, targetSprite);
		AddChild(shotSprite);
		_idToTowerShotSprite[towerShot.Id] = shotSprite;
		return shotSprite;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateShotsToGameState(ConcurrentGameState gameState, Dictionary<Guid, EnemySprite> enemySprites)
	{
		var serverTowerShotIds = gameState.TowerShots.Select(e => e.Id).ToList();
		var spriteTowerShotIds = _idToTowerShotSprite.Keys.ToList();
		foreach (var spriteId in spriteTowerShotIds)
		{
			if (!serverTowerShotIds.Contains(spriteId))
			{
				var toHide = _idToTowerShotSprite[spriteId];
				toHide.ClearModel();
				toHide.Hide();
				_hiddenShots.Enqueue(toHide);
				_idToTowerShotSprite.Remove(spriteId);
			}
		}

		foreach (var towerShot in gameState.TowerShots)
		{
			TowerShotSprite towerShotSprite;
			if (_idToTowerShotSprite.TryGetValue(towerShot.Id, out towerShotSprite))
			{
				towerShotSprite.UpdateModel(towerShot);
			}
			else
			{
				var targetSprite = enemySprites[towerShot.Target.Id];
				towerShotSprite = CreateTowerShotSprite(towerShot, targetSprite);
			}
		}
	}
}
