using System;
using System.Collections.Generic;
using Godot;

public partial class MainTempleSprite : TextureButton, IButtonGroupHandler
{
	private const string Label = "Main Temple";
	private Texture2D _texture;
	private ProgressBar _healthBar;
	private TempleView _templeView;
	private ActionQueue _actionQueue = new();
	public Player Player { get; private set; }

	public void SetModel(Player player)
	{
		Player = player;
		_templeView = new TempleView();
		_templeView.SetModel(0, player);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.MaxValue = Player.HpMax;

		_texture = Resources.MainTempleIcon;

		Pressed += Select;
	}

	private void Select()
	{
		SelectionManager.Instance.Selection = this;
		SelectionManager.Instance.ShowButtonGroup(this, GetButtonContext());
		SelectionManager.Instance.ShowProgressQueue(_texture, Label, Player.Temples[0].TaskQueue);
	}

	private IEnumerable<ButtonContext> GetButtonContext()
	{
		foreach (var bc in _templeView.GetButtonContext())
		{
			yield return bc;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_actionQueue.ExecuteActions();
		_healthBar.Value = Player?.HpCurrent ?? 0;
	}

    public void GridButtonPressed(int row, int column)
    {
        _templeView.GridButtonPressed(row, column);
    }

	public void ProcessModelUpdate(PlayerUpdateInfo info)
	{
		if (_templeView.ProcessModelUpdate(info))
		{
			_actionQueue.Add(RefreshVisuals);
		}
	}

	private void RefreshVisuals()
	{
		if (SelectionManager.Instance.Selection == this)
		{
			Select();
		}
	}
}
