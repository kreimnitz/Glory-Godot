using Godot;
using System;
using System.Collections.Concurrent;
using Utilities.Comms;

public partial class Main : Node, IServerMessageReceivedHandler
{
	private ServerWindow _serverWindow;

	private TopBar _topBar;
	private UiBar _bottomBar;
	private BaseView _baseView;
	private Player _player;

    public override void _EnterTree()
    {
        base._EnterTree();
		ClientMessageManager.CreateSingleton();
		ConnectionManager.CreateSingleton(this);
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_player = new();
		_baseView = GetNode<BaseView>("BaseView");

		_topBar = GetNode<TopBar>("TopBar");
		_topBar.Player = _player;

		_bottomBar = GetNode<UiBar>("BottomBar");
		_bottomBar.ProgressQueueUi.SetTasks(_player.TaskQueue);

		_serverWindow = GetNode<ServerWindow>("ServerWindow");
		_serverWindow.Show();

		var serverButton = GetNode<ButtonContainer>("ServerButtonContainer").Button;
		serverButton.Pressed += _serverWindow.Show;

		SelectionManager.CreateSingleton(_bottomBar);

		_baseView.Initialize(_player);

		_bottomBar.DebugButton0.Pressed +=
			() => ClientMessageManager.Instance.SendMessage(ClientRequestType.DEBUG_SpawnEnemy);
	}

	private ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();
	private void ExecuteActions()
	{
		while (_actions.TryDequeue(out Action tempAction)) tempAction();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		ExecuteActions();
	}

	public void HandleServerMessage(Message message)
    {
		if (message.MessageTypeId == 0)
		{
			var serverGameState = SerializationUtilities.FromByteArray<GameStateInfo>(message.Data);
			var updateInfo = _player.UpdateAndGetInfo(serverGameState.PlayerInfo);
			_actions.Enqueue(() => _baseView.ProcessModelUpdate(updateInfo));
		}
    }
}