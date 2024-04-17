using Godot;
using System;
using System.Collections.Generic;

public partial class ButtonRow : HBoxContainer
{
	public const int ButtonCount = 5;

	public List<QueueButtonContainer> ButtonsContainers { get; private set; } = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < ButtonCount; i++)
		{
			var button = GetNode<QueueButtonContainer>($"Button{i}");
			button.Hide();
			ButtonsContainers.Add(button);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
