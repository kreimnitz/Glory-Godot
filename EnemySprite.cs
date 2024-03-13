using Godot;
using System;
using System.IO;

public partial class EnemySprite : Sprite2D
{
	private PathFollow2D _pathFollow;
	private int _speed = 200;

	public float ProgressRatio => _pathFollow.ProgressRatio;

	private static PackedScene _scene = GD.Load<PackedScene>("res://EnemySprite.tscn");
	public static EnemySprite CreateEnemy(Path2D path)
	{
		var enemy = _scene.Instantiate() as EnemySprite;
		var pathFollow = new PathFollow2D();
		pathFollow.Loop = false;
		path.AddChild(pathFollow);
		enemy._pathFollow = pathFollow;
		return enemy;
	}

	public void Reset()
	{
		_pathFollow.Progress = 0;
		Position = _pathFollow.Position;
		Show();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = _pathFollow.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_pathFollow.Progress += (float)delta * _speed;
		if (_pathFollow.ProgressRatio >= 1)
		{
			Hide();
		}
		else
		{
			Position = _pathFollow.Position;
		}
	}
}
