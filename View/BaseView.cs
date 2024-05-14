using System.Collections.Generic;
using Godot;

public partial class BaseView : Control
{
	private const int MinorTempleButtonCount = 4;
	private MainTempleSprite _mainTemple;
	private Path2D _enemyPath;
	private List<TempleView> _templeViews = new List<TempleView>();
	private EnemySpriteManager _enemySpriteManager = new();
	private TowerShotSpriteManager _towerShotSpriteManager = new();
	private SummonGateView _summonGateView;

	public void SetModel(Player player)
	{
		_mainTemple.SetModel(player);
		_summonGateView.SetModel(player);
		for (int i = 0; i < MinorTempleButtonCount; i++)
		{
			_templeViews[i].SetModel(i + 1, player);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainTemple = GetNode<MainTempleSprite>("MainTempleSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");
		_enemyPath.Curve = EnemyPath.CreateWindingPathCurve();
		_summonGateView = GetNode<SummonGateView>("SummonGate");
		for (int i = 0; i < MinorTempleButtonCount; i++)
		{
			_templeViews.Add(GetNode<TempleView>($"Temple{i + 1}"));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var templeView in _templeViews)
		{
			templeView._Process();
		}
	}

	public void ProcessModelUpdate(PlayerUpdateInfo info)
	{
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

		_mainTemple.ProcessModelUpdate(info);
		foreach (var templeView in _templeViews)
		{
			templeView.ProcessModelUpdate(info);
		}
		_summonGateView.ProcessModelUpdate(info);
	}
}