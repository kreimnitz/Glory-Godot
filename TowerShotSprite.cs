using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class TowerShotSprite : Sprite2D
{
	private Dictionary<int, TowerShotSprite> _modelIdToSpite = new();
	private double _duration = 500; // milliseconds
	private DateTime _creationTime;

	private Vector2 _lastTargetPosition;
	public Node2D Target { get; private set; }
	public Node2D Source { get; private set; }
	public int Id { get; }


	private static PackedScene _scene = GD.Load<PackedScene>("res://TowerShotSprite.tscn");
	public static TowerShotSprite CreateTowerShotSprite(Node2D source, Node2D target)
	{
		var shot = _scene.Instantiate() as TowerShotSprite;
		shot.Source = source;
		shot.Target = target;
		shot._creationTime = DateTime.UtcNow;
		return shot;
	}

	public void Reset(Node2D target)
	{
		Target = target;
		_creationTime = DateTime.UtcNow;
		Show();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var percent = (DateTime.UtcNow - _creationTime).TotalMilliseconds / _duration;
		if (percent > 1)
		{
			percent = 1;
			Hide();
		}

		UpdateLastTargetPosition();
		var offset = _lastTargetPosition - Source.Position;
		Position = offset * (float)percent;
	}

	private void UpdateLastTargetPosition()
	{
		if (Target is not null && Target.Visible)
		{
			_lastTargetPosition = Target.Position;
		}
		else
		{
			Target = null;
		}
	}
}
