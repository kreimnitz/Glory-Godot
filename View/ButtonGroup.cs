using Godot;
using System;
using System.Collections.Generic;

public partial class ButtonGroup : VBoxContainer
{
	public const int RowCount = 4;
	public const int ColumnCount = ButtonRow.ButtonCount;

	private IButtonGroupHandler _handler;

	public void SetHandlerAndVisuals(IButtonGroupHandler handler, IEnumerable<ButtonContext> buttonContexts)
	{
		Clear();
		_handler = handler;
		foreach (var c in buttonContexts)
		{
			var container = _buttonContainers[c.Row][c.Column];
			container.Button.TextureNormal = c.Texture;
			container.QueueInfo = c.LabelInfo;
			container.Show();
		}
	}

	private QueueButtonContainer[][] _buttonContainers = new QueueButtonContainer[RowCount][]; 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < RowCount; i++)
		{
			_buttonContainers[i] = new QueueButtonContainer[ColumnCount];
			var row = GetNode<ButtonRow>($"ButtonRow{i}");
			for (int j = 0; j < ColumnCount; j++)
			{
				var container = row.ButtonsContainers[j];
				_buttonContainers[i][j] = container;
				var tempI = i;
				var tempJ = j;
				container.Button.Pressed += () => InvokeAction(tempI, tempJ);
			}
		}
	}

	private void InvokeAction(int row, int column)
	{
		if (_handler != null)
		{
			_handler.Execute(row, column);
		}
	}

	public void Clear()
	{
		_handler = null;
		for (int i = 0; i < RowCount; i++)
		{
			for (int j = 0; j < ColumnCount; j++)
			{
				_buttonContainers[i][j].Hide();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

public interface IButtonGroupHandler
{
	public void Execute(int row, int column);
}

public class ButtonContext
{
	public int Row { get; set; }
	public int Column { get; set; }
	public Texture2D Texture { get; set; }
	public IProgressInfo LabelInfo { get; set; }
}
