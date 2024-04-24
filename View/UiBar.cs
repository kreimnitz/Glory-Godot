using System.Collections.Generic;
using Godot;

public partial class UiBar : Control
{
	public ProgressQueueUi ProgressQueueUi { get; private set; }

	public ButtonGroup ButtonGroup { get; private set; }

	public TextureButton DebugButton0 { get; private set; }
	public TextureButton DebugButton1 { get; private set; }
	public TextureButton DebugButton2 { get; private set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DebugButton0 = GetNode<ButtonContainer>("DebugButtonContainer0").Button;
		DebugButton1 = GetNode<ButtonContainer>("DebugButtonContainer1").Button;
		DebugButton2 = GetNode<ButtonContainer>("DebugButtonContainer2").Button;

		ProgressQueueUi = GetNode<ProgressQueueUi>("ProgressQueueUi");
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
