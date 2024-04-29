using Godot;
using System;
using System.Collections.Generic;

public partial class BaseView : Control
{
	private Player _player;
	private MainTempleSprite _tower;
	private Path2D _enemyPath;
	private TempleView[] _templeViews = new TempleView[Player.TempleCount];
	private EnemySpriteManager _enemySpriteManager = new();
	private TowerShotSpriteManager _towerShotSpriteManager = new();

	public void Initialize(Player player)
	{
		_player = player;
		_tower.Player = player;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GetNode<MainTempleSprite>("TowerSprite");
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

	public void ProcessModelUpdate(PlayerUpdateInfo info)
	{
		if (info.NewTemples)
		{
			CreateTempleViews();
		}

		foreach (var enemy in info.EnemyUpdates.Added)
		{
			_enemySpriteManager.CreateSprite(enemy, _enemyPath);
		}

		foreach (var enemy in info.EnemyUpdates.Removed)
		{
			_enemySpriteManager.RecycleSprite(enemy.Id);
		}

		foreach (var towerShot in info.TowerShotUpdates.Added)
		{
			_towerShotSpriteManager.CreateSprite(towerShot, this);
		}

		foreach (var towerShot in info.TowerShotUpdates.Removed)
		{
			_towerShotSpriteManager.RecycleSprite(towerShot.Id);
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