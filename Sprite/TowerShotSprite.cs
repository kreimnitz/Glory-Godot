using Godot;

public partial class TowerShotSprite : Sprite2D
{
	public TowerShot Model { get; set; }

	private static PackedScene _scene = GD.Load<PackedScene>("res://Sprite/TowerShotSprite.tscn");
	public static TowerShotSprite CreateTowerShotSprite(TowerShot model)
	{
		var shot = _scene.Instantiate() as TowerShotSprite;
		shot.Model = model;
		return shot;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UpdatePosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if (Model != null)
		{
			Position = new Vector2(Model.PositionX, Model.PositionY);
		}
	}
}
