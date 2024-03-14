using Godot;

public partial class TowerShotSprite : Sprite2D
{
	private Vector2 _lastTargetPosition = new();
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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Visible)
		{
			return;
		}

		if (TargetSprite is null || !TargetSprite.Visible)
		{
			Hide();
			return;
		}

		var offset = TargetSprite.Position - TowerSprite.Position;
		Position = offset * (float)Model.ProgressRatio;
	}
}
