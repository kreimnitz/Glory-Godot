using Godot;
using System;
using System.IO;

public partial class EnemySprite : Sprite2D
{
	private PathFollow2D _pathFollow;
	private int _speed = 200;

	public float ProgressRatio => _pathFollow.ProgressRatio;

	public Enemy Model { get; private set; }

	private static PackedScene _scene = GD.Load<PackedScene>("res://Sprite/EnemySprite.tscn");
	public static EnemySprite CreateEnemy(Enemy model, Path2D path)
	{
		var enemy = _scene.Instantiate() as EnemySprite;
		enemy.Model = model;
		var pathFollow = new PathFollow2D();
		pathFollow.Loop = false;
		path.AddChild(pathFollow);
		enemy._pathFollow = pathFollow;
		return enemy;
	}

	public void Kill()
	{
		Hide();
		Model = null;
	}

	public void Reset(Enemy model)
	{
		Model = model;
		Show();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Visible)
		{
			return;
		}

		_pathFollow.ProgressRatio = (float)Model.ProgressRatio;
		Position = _pathFollow.Position;
	}
}
