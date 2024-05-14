using Godot;
using System;

public partial class ButtonContainer : MarginContainer
{
	private TextureRect _grayMask;
	public TextureButton Button { get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button = GetNode<TextureButton>("TextureButton");
		_grayMask = GetNode<TextureRect>("DisabledMask");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Button.Disabled && !_grayMask.Visible)
		{
			_grayMask.Show();
		}
		if (!Button.Disabled && _grayMask.Visible)
		{
			_grayMask.Hide();
		}
	}
}
