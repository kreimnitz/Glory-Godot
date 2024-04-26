using Godot;
using System;
using System.Collections.Generic;

public partial class TowerSprite : Sprite2D
{
	private ProgressBar _healthBar;
	private PackedScene _towerShotScene;
	private Dictionary<Guid, TowerShotSprite> _idToTowerShotSprite = new();
	private Queue<TowerShotSprite> _hiddenShots = new Queue<TowerShotSprite>();

	public Player Player { get; set; }

    public TowerShotSprite CreateTowerShotSprite(TowerShot towerShot, EnemySprite targetSprite, Node parent)
	{
		if (_hiddenShots.TryDequeue(out TowerShotSprite toRecycle))
		{
			toRecycle.Reset(towerShot, this, targetSprite);
			_idToTowerShotSprite[towerShot.Id] = toRecycle;
			return toRecycle;
		}

		var shotSprite = TowerShotSprite.CreateTowerShotSprite(towerShot, this, targetSprite);
		shotSprite.Hidden += () => _hiddenShots.Enqueue(shotSprite);
		_idToTowerShotSprite[towerShot.Id] = shotSprite;
		parent.AddChild(shotSprite);
		return shotSprite;
	}

	public void KillTowerShotSprite(Guid id)
	{
		if (_idToTowerShotSprite.TryGetValue(id, out TowerShotSprite towerShotSprite))
		{
			towerShotSprite.Kill();
			_idToTowerShotSprite.Remove(id);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		area.InputEvent += HandleInputEvent;

		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.MaxValue = Player.HpMax;
	}

    private void HandleInputEvent(Node viewport, InputEvent e, long shapeIndex)
	{
		if (ViewUtilites.IsLeftMouseRelease(e))
		{
			// select tower
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_healthBar.Value = Player?.HpCurrent ?? 0;
	}
}
