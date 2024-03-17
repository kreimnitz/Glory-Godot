using Godot;
using System;

public partial class TopBar : Control
{
	private Label _gloryLabel;
	private const string GloryLabelPrefix = "Glory: ";

	private Label _followersLabel;
	private const string FollowersLabelPrefix = "Followers: ";

	public PlayerInfo Player { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gloryLabel = GetNode<Label>("GloryLabel");
		_followersLabel = GetNode<Label>("FollowersLabel");
		Player = new();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_gloryLabel.Text = GloryLabelPrefix + Player.Glory;
		_followersLabel.Text = FollowersLabelPrefix + Player.FollowerCount;
	}
}
