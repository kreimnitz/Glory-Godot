using Godot;
using System;
using System.Collections.Generic;

public partial class ProgressQueueUi : Control
{
	private const int ButttonCount = 5;
	private List<ProgressItem> _taskList = new();

	private ProgressBar _progressBar;

	private TextureRect _mainTexture;
	private TextureButton[] _buttons = new TextureButton[ButttonCount];

	public void SetTasks(List<ProgressItem> playerInfo)
	{
		_taskList = playerInfo;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainTexture = GetNode<TextureRect>("TextureRect");
		_mainTexture.Texture = Resources.DemigodIcon;

		_progressBar = GetNode<ProgressBar>("ProgressBar");
		for (int i = 0; i < ButttonCount; i++)
		{
			_buttons[i] = GetNode<ButtonContainer>("ButtonContainer" + i).Button;
			_buttons[i].TextureNormal = Resources.FollowerIcon;
			_buttons[i].TextureDisabled = Resources.BlankIcon;
			_buttons[i].Disabled = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_taskList.Count > 0)
		{
			_progressBar.Value = _taskList[0].ProgressRatio * 100;
		}
		else 
		{
			_progressBar.Value = 0;
		}
		for (int i = 0; i < ButttonCount; i++)
		{
			if (i >= _taskList.Count)
			{
				_buttons[i].Disabled = true;
			}
			else
			{
				_buttons[i].Disabled = false;
			}
		}
	}
}
