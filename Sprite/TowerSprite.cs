using Godot;
using System;
using System.Collections.Generic;

public partial class TowerSprite : Sprite2D
{
	private PackedScene _towerShotScene;
	private Dictionary<Guid, TowerShotSprite> _idToTowerShotSprite = new();

	private Queue<TowerShotSprite> _hiddenShots = new Queue<TowerShotSprite>();

    public TowerShotSprite CreateTowerShotSprite(TowerShot towerShot, EnemySprite targetSprite)
	{
		if (_hiddenShots.TryDequeue(out TowerShotSprite toRecycle))
		{
			toRecycle.Reset(towerShot, this, targetSprite);
			_idToTowerShotSprite[towerShot.Id] = toRecycle;
			return toRecycle;
		}

		var shotSprite = TowerShotSprite.CreateTowerShotSprite(towerShot, this, targetSprite);
		AddChild(shotSprite);
		shotSprite.Hidden += () => _hiddenShots.Enqueue(shotSprite);
		_idToTowerShotSprite[towerShot.Id] = shotSprite;
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
	}
}
