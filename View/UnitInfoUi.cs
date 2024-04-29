using Godot;
using System;
using System.Collections.Generic;

public partial class UnitInfoUi : Control
{
	private TextureRect _mainTexture;
	private ProgressBar _hpBar;
	private Label _mainLabel;
	private Label _hpLabel;

	private IUnitModel _model;

	public void SetModel(IUnitModel unitModel)
	{
		_model = unitModel;

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
	}
}

public static class UnitHelpers
{
	public static Dictionary<UnitType, (string, Texture2D)> UnitVisuals = new Dictionary<UnitType, (string, Texture2D)>
	{
		{UnitType.FireImp, ("Fire Imp", Resources.ImpIcon)}
	};
}
