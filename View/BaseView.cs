using Godot;
using System;
using System.Collections.Generic;

public partial class BaseView : Control
{
	private TowerSprite _tower;
	private Path2D _enemyPath;
	private const int TempleCount = 3;
	private TextureButton[] _templeButtons = new TextureButton[3];
	private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();

	public void Initialize(Player player, SelectionManager selectionManager)
	{
		for (int i = 0; i < TempleCount; i++)
		{
			_templeButtons[i].Pressed += () => selectionManager.SelectTemple(player, i);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");
		for (int i = 0; i < TempleCount; i++)
		{
			var templeButton = GetNode<TextureButton>($"Temple{i}");
			_templeButtons[i] = templeButton;
		}
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