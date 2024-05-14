using Godot;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public partial class SummonGateView : TextureButton, IButtonGroupHandler
{
	private Player _player;

	private static List<SummonButtonInfo> _normalTypePositionList = new()
	{
		new (0, 0, Enemies.WarriorInfo),
	};

	private static List<SummonButtonInfo> _fireTypePositionList = new()
	{
		new (0, 0, Enemies.FireImpInfo),
	};

	private SummonGateButtonManager _fireButtonManager;
	private SummonGateButtonManager _normalButtonManager;
	private TextureButton _fireButton;
	private Element _selectedElement;
	private ActionQueue _actionQueue = new();

	public void SetModel(Player player)
	{
		_player = player;
		foreach (var spawner in _player.SummonGate.Spawners)
		{
			ConnectSpawner(spawner);
		}
		_actionQueue.Add(RefreshVisuals);
	}

	public void ProcessModelUpdate(PlayerUpdateInfo info)
	{
		foreach (var spawner in info.SummonGateUpdates.SpawnerUpdates.Added)
		{
			ConnectSpawner(spawner);
		}
		if (info.ElementAdded || !info.AddedTech.IsEmpty())
		{
			_actionQueue.Add(RefreshVisuals);
		}
	}

    private void ConnectSpawner(Spawner spawner)
	{
		var info = Enemies.TypeToInfo[spawner.UnitType];
		var buttonManager = GetButtonManagerForElement(info.Element);
		buttonManager.AddOrUpdateSpawner(spawner);
	}


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_fireButton = GetNode<TextureButton>("FireButton");
		_fireButton.Pressed += () => SelectElement(Element.Fire);
		Pressed += () => SelectElement(Element.None);
		_selectedElement = Element.None;
		_fireButtonManager = new(Element.Fire, _fireTypePositionList);
		_normalButtonManager = new(Element.None, _normalTypePositionList);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_actionQueue.ExecuteActions();
	}

	private void SelectElement(Element element)
	{
		_selectedElement = element;
		Select();
	}

	private SummonGateButtonManager GetButtonManagerForElement(Element element)
	{
		switch (element)
		{
			case Element.Fire:
			{
				return _fireButtonManager;
			}
			default:
			{
				return _normalButtonManager;
			}
		}
	}

	private void Select()
	{
		SelectionManager.Instance.Selection = this;
		SelectionManager.Instance.ShowButtonGroup(this, GetButtonContexts());
		SelectionManager.Instance.ShowProgressQueue(Resources.SummonGateIcon, "Summoning Gate", _player.SummonGate.TaskQueue);
	}

	private void RefreshVisuals()
	{
		_fireButton.Hide();
		if (_player.Temples.Any(t => t.Element == Element.Fire))
		{
			_fireButton.Show();
		}
		if (SelectionManager.Instance.Selection == this)
		{
			Select();
		}
	}

	private IEnumerable<ButtonContext> GetButtonContexts()
	{
		return GetButtonManagerForElement(_selectedElement).GetButtonContexts();
	}

    public void GridButtonPressed(int row, int column)
    {
		var buttonManager = GetButtonManagerForElement(_selectedElement);
		buttonManager.ExecuteButton(row, column);
	}

	private class SummonGateButtonManager
	{
		private Dictionary<(int, int), SummonButtonInfo> _buttonInfos = new();

		public Element Element { get; set; }

		public SummonGateButtonManager(Element element, List<SummonButtonInfo> infos)
		{
			Element = element;
			foreach (var info in infos)
			{
				_buttonInfos.Add((info.Row, info.Column), info);
			}
		}

		public void AddOrUpdateSpawner(Spawner spawner)
		{
			var info = _buttonInfos.Values.First(i => i.EnemyInfo.Type == spawner.UnitType);
			info.Spawner = spawner;
		}

		public void ExecuteButton(int row, int column)
		{
			var info = TryGetButtonInfo(row, column);
			if (info is null)
			{
				return;
			}
			if (info.Spawner is null)
			{
				ClientMessageManager.Instance.SendEnemyTechRequest(info.EnemyInfo.Type);
			}
			else
			{
				ClientMessageManager.Instance.SendSummonRequest(info.EnemyInfo.Type);
			}
		}

		private SummonButtonInfo TryGetButtonInfo(int row, int column)
		{
			if (_buttonInfos.TryGetValue((row, column), out var buttonInfo))
			{
				return buttonInfo;
			}
			return null;
		}

		public IEnumerable<ButtonContext> GetButtonContexts()
		{
			foreach (var buttonInfo in _buttonInfos.Values)
			{
				Texture2D texture;
				string tooltip;
				texture = ResourceHelpers.UnitTypeToIcon[buttonInfo.EnemyInfo.Type];
				if (buttonInfo.Spawner == null)
				{
					tooltip = CreateResearchTooltip(buttonInfo.EnemyInfo.Type);
				}
				else
				{
					tooltip = CreateSummonTooltip(buttonInfo.EnemyInfo.Type);
				}
				yield return new ButtonContext(buttonInfo.Row, buttonInfo.Column, texture, tooltip, buttonInfo.Spawner);
			}
		}

		private string CreateResearchTooltip(UnitType type)
		{
			var info = Enemies.TypeToInfo[type];
			return $"Research {info.Name}\nCost: {info.RequiredTech.GloryCost}";
		}

		private string CreateSummonTooltip(UnitType type)
		{
			var info = Enemies.TypeToInfo[type];
			return $"Summon {info.Name}\nCost: {info.GloryCost}";
		}
	}

	private class SummonButtonInfo
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public EnemyInfo EnemyInfo { get; set; }
		public Spawner Spawner { get; set; }

		public SummonButtonInfo(int row, int column, EnemyInfo info)
		{
			Row = row;
			Column = column;
			EnemyInfo = info;
		}
	}
}
