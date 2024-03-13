using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TowerSprite : Sprite2D
{
	private PackedScene _towerShotScene;
	private DateTime _lastShotTime = DateTime.MinValue;
	private int _cooldownMs = 500;

	public int Range { get; } = 200;
	public int Damage { get; }

	private Queue<TowerShotSprite> _hiddenShots = new Queue<TowerShotSprite>();
	public void Shoot(Node2D target)
	{
		if (_hiddenShots.TryDequeue(out TowerShotSprite toRecycle))
		{
			toRecycle.Reset(target);
			return;
		}

		var shot = TowerShotSprite.CreateTowerShotSprite(this, target);
		shot.Hidden += () => _hiddenShots.Enqueue(shot);
		AddChild(shot);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void FireAtEnemies(IEnumerable<EnemySprite> enemies)
    {
		if ((DateTime.UtcNow - _lastShotTime).TotalMilliseconds < _cooldownMs)
		{
			return;
		}

        var closestEnemy = enemies.OrderByDescending(e => e.ProgressRatio).First();
		if ((closestEnemy.Position - Position).Length() <= Range)
		{
			Shoot(closestEnemy);
			_lastShotTime = DateTime.UtcNow;
		}
    }
}
