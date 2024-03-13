using Godot;
using System;

public partial class TopBar : Control
{
	private Label _label;
	private const string GloryLabelPrefix = "Glory: ";

	public Player Player { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label>("ValueLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_label.Text = GloryLabelPrefix + Player.Glory;
	}
}
