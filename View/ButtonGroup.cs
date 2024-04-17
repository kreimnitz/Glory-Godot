using Godot;
using System;
using System.Collections.Generic;

public partial class ButtonGroup : VBoxContainer
{
	public const int RowCount = 4;
	public const int ColumnCount = ButtonRow.ButtonCount;

	public void SetContext(IEnumerable<ButtonContext> buttonContexts)
	{
		Clear();
		foreach (var c in buttonContexts)
		{
			_actions[c.Row][c.Column] = c.Action;
			var container = _buttonContainers[c.Row][c.Column];
			container.Button.TextureNormal = c.Texture;
			container.Show();
			container.QueueInfo = c.LabelInfo;
		}
	}

	private QueueButtonContainer[][] _buttonContainers = new QueueButtonContainer[RowCount][]; 
	private Action[][] _actions = new Action[RowCount][];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < RowCount; i++)
		{
			_actions[i] = new Action[ColumnCount];
			_buttonContainers[i] = new QueueButtonContainer[ColumnCount];
			var row = GetNode<ButtonRow>($"ButtonRow{i}");
			for (int j = 0; j < ColumnCount; j++)
			{
				var container = row.ButtonsContainers[j];
				_buttonContainers[i][j] = container;
				container.Button.Pressed += () => InvokeAction(i, j);
			}
		}
	}

	private void InvokeAction(int row, int column)
	{
		if (_actions[row][column] != null)
		{
			_actions[row][column]();
		}
	}

	private void Clear()
	{
		for (int i = 0; i < RowCount; i++)
		{
			for (int j = 0; j < ColumnCount; j++)
			{
				_buttonContainers[i][j].Hide();
				_actions[i][j] = null;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

public class ButtonContext
{
	public int Row { get; }
	public int Column { get; }
	public Action Action { get; }
	public Texture2D Texture { get; }
	public IProgressInfo LabelInfo { get; }
}
