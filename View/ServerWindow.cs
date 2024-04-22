using Godot;

public partial class ServerWindow : Window
{
	private ConnectionManager _connectionManager;
	private CheckBox _soloCheckBox;
	private Button _startGameButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_connectionManager = ConnectionManager.Instance;
		CloseRequested += Hide;

		_soloCheckBox = GetNode<CheckBox>("SoloCheckBox");
		var createServerButton = GetNode<Button>("CreateServerButton");
		createServerButton.Pressed += CreateServer;

		var connectButton = GetNode<Button>("ConnectButton");
		connectButton.Pressed += _connectionManager.ConnectToServer;

		_startGameButton = GetNode<Button>("StartGameButton");
		_startGameButton.Pressed += _connectionManager.StartGame;
		_startGameButton.Disabled = true;
	}

	private void CreateServer()
	{
		_connectionManager.CreateServer(_soloCheckBox.ButtonPressed);
		_startGameButton.Disabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
