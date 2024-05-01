using System.Net;
using System.Threading.Tasks;
using Godot;

public partial class ServerWindow : Window
{
	private const string PlayersConnectedStatus = "Server Status: All players connected!";
	private const string NotConnectedStatus = "Server Status: No local server";
	private const string WaitingStatus = "Server Status: Waiting for remote player...";

	private ConnectionManager _connectionManager;
	private CheckBox _soloCheckBox;
	private Button _startGameButton;
	private LineEdit _ipAddressInput;
	private Label _serverStatusLabel;
	private Label _publicIpLabel;
	private string _ipAddress;
	private bool _publicIpLabelSet = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_connectionManager = ConnectionManager.Instance;
		CloseRequested += Hide;

		_soloCheckBox = GetNode<CheckBox>("SoloCheckBox");
		var createServerButton = GetNode<Button>("CreateServerButton");
		createServerButton.Pressed += CreateServer;

		_ipAddressInput = GetNode<LineEdit>("IPAddressInput");

		var connectButton = GetNode<Button>("ConnectButton");
		connectButton.Pressed += () => _connectionManager.ConnectToServer(_ipAddressInput.Text);

		_startGameButton = GetNode<Button>("StartGameButton");
		_startGameButton.Pressed += StartGame;
		_startGameButton.Disabled = true;

		_serverStatusLabel = GetNode<Label>("ServerStatusLabel");
		_serverStatusLabel.Text = NotConnectedStatus;

		_publicIpLabel = GetNode<Label>("PublicIpLabel");
		var ignoredTask = GetExternalIpAddressAsync();
	}

	public async Task GetExternalIpAddressAsync()
	{
		var externalIpString = (await new System.Net.Http.HttpClient().GetStringAsync("http://icanhazip.com"))
			.Replace("\\r\\n", "").Replace("\\n", "").Trim();
		if (!IPAddress.TryParse(externalIpString, out var ipAddress)) return;
		_ipAddress = ipAddress.ToString();
	}

	private void StartGame()
	{
		_connectionManager.StartGame();
		Hide();
	}

	private void CreateServer()
	{
		_serverStatusLabel.Text = WaitingStatus;
		_connectionManager.CreateServer(_soloCheckBox.ButtonPressed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_publicIpLabelSet && _ipAddress is not null)
		{
			_publicIpLabelSet = true;
			_publicIpLabel.Text = $"Your Public IP: {_ipAddress}";
		}
		if (_connectionManager.Connected && _serverStatusLabel.Text != PlayersConnectedStatus)
		{
			_serverStatusLabel.Text = PlayersConnectedStatus;
			_startGameButton.Disabled = _connectionManager.Server == null;
		}
	}
}
