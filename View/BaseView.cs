using Godot;
using System;
using System.Collections.Generic;

public partial class BaseView : Control
{
	private TowerSprite _tower;
	private Path2D _enemyPath;

	private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Queue<EnemySprite> _hiddenEnemies = new Queue<EnemySprite>();
	private EnemySprite CreateEnemySprite(Enemy model)
	{
		if (_hiddenEnemies.TryDequeue(out EnemySprite toRecycle))
		{
			toRecycle.Reset(model);
			return toRecycle;
		}

		var enemy = EnemySprite.CreateEnemy(model, _enemyPath);
		AddChild(enemy);
		enemy.Hidden += () => _hiddenEnemies.Enqueue(enemy);
		return enemy;
	}

	public void CreateSpritesFromUpdate(PlayerUpdateInfo info)
	{
		foreach (var enemy in info.EnemyUpdates.Added)
		{
			var sprite = CreateEnemySprite(enemy);
			_enemyIdToSprite[enemy.Id] = sprite;
		}

		foreach (var enemy in info.EnemyUpdates.Removed)
		{
			if (_enemyIdToSprite.TryGetValue(enemy.Id, out EnemySprite enemySprite))
			{
				enemySprite.Kill();
				_enemyIdToSprite.Remove(enemy.Id);
			}
		}

		foreach (var towerShot in info.TowerShotUpdates.Added)
		{
			if (_enemyIdToSprite.TryGetValue(towerShot.TargetId, out EnemySprite enemySprite))
			{
				_tower.CreateTowerShotSprite(towerShot, enemySprite);
			}
		}

		foreach (var towerShot in info.TowerShotUpdates.Removed)
		{
			_tower.KillTowerShotSprite(towerShot.Id);
		}
	}
}
