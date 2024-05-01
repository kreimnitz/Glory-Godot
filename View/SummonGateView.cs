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
		_player.Tech.OnTechUpdate += TechUpdated;
		foreach (var temple in _player.Temples)
		{
			temple.PropertyChanged += TempleUpdated;
		}
		_player.SummonGate.SpawnersAdded += SpawnersAdded;
		foreach (var spawner in _player.SummonGate.Spawners)
		{
			ConnectSpawner(spawner);
		}
		_actionQueue.Add(RefreshVisuals);
	}

    private void TempleUpdated(object sender, PropertyChangedEventArgs e)
    {
		if (e.PropertyName == nameof(Temple.Element))
		{
			_actionQueue.Add(RefreshVisuals);
		}
	}

    private void TechUpdated(object sender, TechUpdateEventArgs e)
    {
        _actionQueue.Add(RefreshVisuals);
    }

    private void SpawnersAdded(object sender, SpawnersAddedEventArgs e)
    {
        foreach (var spawner in e.Added)
		{
			ConnectSpawner(spawner);
		}
		_actionQueue.Add(RefreshVisuals);
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
		SelectionManager.Instance.ShowInfoUi(Resources.SummonGateIcon, "Summoning Gate");
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
		var unitType = buttonManager.TryGetType(row, column);
		if (unitType != null)
		{
			ClientMessageManager.Instance.SendSummonRequest(unitType.Value);
		}
	}

	private class SummonGateButtonManager
	{
		private List<SummonButtonInfo> _buttonInfos;

		public Element Element { get; set; }

		public SummonGateButtonManager(Element element, List<SummonButtonInfo> infos)
		{
			Element = element;
			_buttonInfos = infos;
		}

		public void AddOrUpdateSpawner(Spawner spawner)
		{
			var info = _buttonInfos.First(i => i.EnemyInfo.Type == spawner.UnitType);
			info.Spawner = spawner;
		}

		public UnitType? TryGetType(int row, int column)
		{
			return _buttonInfos.FirstOrDefault(i => i.Row == row && i.Column == column)?.EnemyInfo.Type;
		}

		public IEnumerable<ButtonContext> GetButtonContexts()
		{
			foreach (var buttonInfo in _buttonInfos.Where(i => i.Spawner != null))
			{
				var texture = ResourceHelpers.UnitTypeToIcon[buttonInfo.EnemyInfo.Type];
				var tooltip = CreateTooltip(buttonInfo.EnemyInfo.Type);
				var bc = new ButtonContext(buttonInfo.Row, buttonInfo.Column, texture, tooltip, buttonInfo.Spawner);
				yield return bc;
			}
		}

		private string CreateTooltip(UnitType type)
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
