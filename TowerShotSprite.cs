using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class TowerShotSprite : Sprite2D
{
	public TowerShot Model { get; private set; }

	public TowerSprite TowerSprite { get; private set; }
	public EnemySprite TargetSprite { get; private set; }

	private static PackedScene _scene = GD.Load<PackedScene>("res://TowerShotSprite.tscn");
	public static TowerShotSprite CreateTowerShotSprite(TowerShot model, TowerSprite towerSprite, EnemySprite targetSprite)
	{
		var shot = _scene.Instantiate() as TowerShotSprite;
		shot.Model = model;
		shot.TowerSprite = towerSprite;
		shot.TargetSprite = targetSprite;
		return shot;
	}

	public void Reset(TowerShot towerShot)
	{
		Model = towerShot;
		Show();
	}

	public void ClearModel()
	{
		Model = null;
	}

	public void UpdateModel(TowerShot towerShot)
	{
		Model.UpdateFrom(towerShot);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Model is null)
		{
			if (Visible)
			{
				Hide();
			}
			return;
		}
		var offset = TargetSprite.Position - TowerSprite.Position;
		Position = offset * (float)Model.ProgressRatio;
	}
}
