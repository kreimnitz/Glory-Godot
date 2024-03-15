using Godot;

public partial class TowerShotSprite : Sprite2D
{
	private Vector2 _lastTargetPosition = new();
	public TowerShot Model { get; private set; }

	public TowerSprite TowerSprite { get; private set; }
	public EnemySprite TargetSprite { get; private set; }

	private static PackedScene _scene = GD.Load<PackedScene>("res://Sprite/TowerShotSprite.tscn");
	public static TowerShotSprite CreateTowerShotSprite(TowerShot model, TowerSprite towerSprite, EnemySprite targetSprite)
	{
		var shot = _scene.Instantiate() as TowerShotSprite;
		shot.Model = model;
		shot.TowerSprite = towerSprite;
		shot.TargetSprite = targetSprite;
		return shot;
	}

	public void Kill()
	{
		Model = null;
		TargetSprite = null;
		TowerSprite = null;
		Hide();
	}

	public void Reset(TowerShot towerShot, TowerSprite towerSprite, EnemySprite targetSprite)
	{
		TowerSprite = towerSprite;
		TargetSprite = targetSprite;
		Model = towerShot;
		Show();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdatePostion();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdatePostion();
	}

	private void UpdatePostion()
	{
		if (!Visible)
		{
			return;
		}

		UpdateLastTargetPosition();
		var offset = _lastTargetPosition - TowerSprite.Position;
		Position = offset * (float)Model.ProgressRatio;
	}

	private void UpdateLastTargetPosition()
	{
		if (TargetSprite is not null && TargetSprite.Visible)
		{
			_lastTargetPosition = TargetSprite.Position;
		}
		else
		{
			TargetSprite = null;
		}
	}
}
