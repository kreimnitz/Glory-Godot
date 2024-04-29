using System;
using System.Collections.Generic;
using Godot;

public partial class MainTempleSprite : TextureButton, IButtonGroupHandler
{
	private const string Label = "Main Temple";
	private Dictionary<(int, int), Action> _buttonActions = new();
	private ButtonContext _recruitFollowerBC;
	private Texture2D _texture;
	private ProgressBar _healthBar;
	public Player Player { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthBar.MaxValue = Player.HpMax;

		_texture = Resources.MainTempleIcon;
		_recruitFollowerBC = new ButtonContext(0, 0, Resources.FollowerIcon, Follower.RecruitTooltip);
		_buttonActions.Add((_recruitFollowerBC.Row, _recruitFollowerBC.Column), RecruitFollower);

		Pressed += Select;
	}

	private void Select()
	{
		SelectionManager.Instance.Selection = this;
		SelectionManager.Instance.ShowButtonGroup(this, GetButtonContext());
		SelectionManager.Instance.ShowProgressQueue(_texture, Label, Player.TaskQueue);
	}

	private IEnumerable<ButtonContext> GetButtonContext()
	{
		yield return _recruitFollowerBC;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_healthBar.Value = Player?.HpCurrent ?? 0;
	}

    public void GridButtonPressed(int row, int column)
    {
        if (_buttonActions.TryGetValue((row, column), out Action action))
		{
			action();
		}
    }

	private void RecruitFollower()
	{
		ClientMessageManager.Instance.SendMessage(ClientRequestType.PlayerRequest);
	}
}
