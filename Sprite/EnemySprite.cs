using Godot;

public partial class EnemySprite : Sprite2D
{
	public static Vector2 EnemyPathOffset = new();

	private PathFollow2D _pathFollow;
	private int _speed = 20;
	private ProgressBar _hpBar;

	public Enemy Model { get; private set; }

	private static PackedScene _scene = GD.Load<PackedScene>("res://Sprite/EnemySprite.tscn");
	public static EnemySprite CreateEnemy(Enemy model, Path2D path)
	{
		var enemy = _scene.Instantiate() as EnemySprite;
		enemy.Model = model;
		var pathFollow = new PathFollow2D();
		pathFollow.Loop = false;
		pathFollow.Rotates = true;
		path.AddChild(pathFollow);
		enemy._pathFollow = pathFollow;
		path.AddChild(enemy);
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
		_hpBar = GetNode<ProgressBar>("HealthBar");
		_hpBar.MaxValue = Model.HpMax;
		Position = _pathFollow.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Visible)
		{
			return;
		}

		_pathFollow.Progress = Model.Progress;
		Position = _pathFollow.Position;

		_hpBar.Value = Model.HpCurrent;
	}
}
