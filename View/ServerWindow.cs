using Godot;
using Utilities.Comms;

public partial class ServerWindow : Window
{
	private IServerMessageReceivedHandler _serverMessageHandler;
	private CheckBox _soloCheckBox;
	private Button _startGameButton;

	public GloryServer Server { get; private set; } = null;

	public ClientMessageTransmitter Client { get; private set; }

	public void SetClientHandler(IServerMessageReceivedHandler serverMessageHandler)
	{
		_serverMessageHandler = serverMessageHandler;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CloseRequested += Hide;

		var createServerButton = GetNode<Button>("CreateServerButton");
		createServerButton.Pressed += CreateServer;

		var connectButton = GetNode<Button>("ConnectButton");
		connectButton.Pressed += ConnectToServer;

		_soloCheckBox = GetNode<CheckBox>("SoloCheckBox");

		_startGameButton = GetNode<Button>("StartGameButton");
		_startGameButton.Pressed += StartGame;
		_startGameButton.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartGame()
	{
		Server?.StartGame();
	}

	private void CreateServer()
	{
		if (Server is not null)
		{
			return;
		}
		Server = new GloryServer(_soloCheckBox.ButtonPressed);
		ConnectToServer();
		_startGameButton.Disabled = false;
	}

	private void ConnectToServer()
	{
		if (Client is not null)
		{
			return;
		}
		Client = new(_serverMessageHandler);
	}
}
