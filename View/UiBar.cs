using Godot;
using System;

public partial class UiBar : Control
{
	public BaseButton AddFollowerButton { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddFollowerButton = GetNode<ButtonContainer>("RecruitFollowerButtonContainer").Button;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
