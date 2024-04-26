using Godot;
using System;
using System.Collections.Generic;

public partial class BaseView : Control
{
	private Player _player;
	private TowerSprite _tower;
	private Path2D _enemyPath;
	private TempleView[] _templeViews = new TempleView[Player.TempleCount];
	private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();

	public void Initialize(Player player)
	{
		_player = player;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");
		_enemyPath.Curve = EnemyPath.CreateWindingPathCurve();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var templeView in _templeViews)
		{
			templeView?._Process();
		}
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
		enemy.Hidden += () => _hiddenEnemies.Enqueue(enemy);
		return enemy;
	}

	public void ProcessModelUpdate(PlayerUpdateInfo info)
	{
		if (info.NewTemples)
		{
			CreateTempleViews();
		}

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
				_tower.CreateTowerShotSprite(towerShot, enemySprite, this);
			}
		}

		foreach (var towerShot in info.TowerShotUpdates.Removed)
		{
			_tower.KillTowerShotSprite(towerShot.Id);
		}
	}

	private void CreateTempleViews()
	{
		for (int i = 0; i < Player.TempleCount; i++)
		{
			var templeButton = GetNode<TextureButton>($"Temple{i}");
			_templeViews[i] = new TempleView(_player.Temples[i], templeButton, _player);
		}
	}
}