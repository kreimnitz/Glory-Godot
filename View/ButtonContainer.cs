using Godot;
using System;

public partial class ButtonContainer : MarginContainer
{
	public TextureButton Button { get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button = GetNode<TextureButton>("TextureButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
