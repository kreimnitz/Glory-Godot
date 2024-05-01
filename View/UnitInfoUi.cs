using Godot;
using System;
using System.Collections.Generic;

public partial class UnitInfoUi : Control
{
	private TextureRect _mainTexture;
	private ProgressBar _hpBar;
	private Label _mainLabel;
	private Label _hpLabel;

	private IProgressInfo _hpBarInfo;

	public void UpdateVisuals(Texture2D texture, string label, IProgressInfo hpInfo = null)
	{
		_mainTexture.Texture = texture;
		_mainLabel.Text = label;
		_hpBarInfo = hpInfo;
		if (hpInfo == null)
		{
			_hpBar.Hide();
			_hpLabel.Hide();
		}
		else
		{
			_hpBarInfo = hpInfo;
			_hpBar.Show();
			_hpLabel.Show();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainTexture = GetNode<TextureRect>("MainTexture");
		_hpBar = GetNode<ProgressBar>("HealthBar");
		_mainLabel = GetNode<Label>("MainLabel");
		_hpLabel = GetNode<Label>("HpLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_hpBarInfo is not null)
		{
			_hpBar.Value = _hpBarInfo.CurrentValue;
			_hpBar.MaxValue = _hpBarInfo.Max;

			_hpLabel.Text = $"{_hpBar.Value} / {_hpBar.MaxValue}";
		}
	}
}

public static class UnitHelpers
{
	public static Dictionary<UnitType, (string, Texture2D)> UnitVisuals = new Dictionary<UnitType, (string, Texture2D)>
	{
		{UnitType.FireImp, ("Fire Imp", Resources.ImpIcon)}
	};
}
