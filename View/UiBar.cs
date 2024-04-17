using System.Collections.Generic;
using Godot;

public partial class UiBar : Control
{
	public ProgressQueueUi ProgressQueueUi { get; private set; }
	public TextureButton AddFollowerButton { get; private set; }

	public TextureButton AddFireTempleButton { get; private set; }

	public TextureButton AddVentButton { get; private set; }

	public ButtonGroup ButtonGroup { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddFollowerButton = GetNode<ButtonContainer>("RecruitFollowerButtonContainer").Button;
		AddFollowerButton.TextureNormal = Resources.FollowerIcon;

		ProgressQueueUi = GetNode<ProgressQueueUi>("ProgressQueueUi");

		AddFireTempleButton = GetNode<ButtonContainer>("AddFireTempleButtonContainer").Button;
		AddVentButton = GetNode<ButtonContainer>("AddVentButtonContainer").Button;

		ButtonGroup = GetNode<ButtonGroup>("MainButtonGroup");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ClearSelection()
	{
		ProgressQueueUi.Hide();
		ButtonGroup.Hide();
	}
}
