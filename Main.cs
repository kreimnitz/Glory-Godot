using Godot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Utilities.Comms;

public partial class Main : Node, IServerMessageReceivedHandler
{
	private ServerWindow _serverWindow;
	private TowerSprite _tower;
	private Path2D _enemyPath;
	private TopBar _topBar;
	private UiBar _bottomBar;

	private Player _player;

	private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = new();
		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");

		_topBar = GetNode<TopBar>("TopBar");
		_topBar.Player = _player;

		_bottomBar = GetNode<UiBar>("BottomBar");
		_bottomBar.AddFollowerButton.Pressed += TryAddFollower;
		_bottomBar.ProgressQueueUi.SetTasks(_player.TaskQueue);

		_serverWindow = GetNode<ServerWindow>("ServerWindow");
		_serverWindow.SetClientHandler(this);
		_serverWindow.Show();

		var serverButton = GetNode<ButtonContainer>("ServerButtonContainer").Button;
		serverButton.Pressed += _serverWindow.Show;
	}

    private void TryAddFollower()
    {
		var message = new Message((int)ClientRequests.AddFollower, Array.Empty<byte>());
        _serverWindow.Client.SendMessage(message);
    }

    private Queue<EnemySprite> _hiddenEnemies = new Queue<EnemySprite>();
    private EnemySprite CreateEnemySprite(Enemy model)
    {
		if (_hiddenEnemies.TryDequeue(out EnemySprite toRecycle))
		{
			toRecycle.Reset(model);
			return toRecycle;
		}

		var enemy = EnemySprite.CreateEnemy(model, _enemyPath);
		AddChild(enemy);
		enemy.Hidden += () => _hiddenEnemies.Enqueue(enemy);
		return enemy;
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
			_actions.Enqueue(() => CreateSpritesFromUpdate(updateInfo));
		}
    }
	private void CreateSpritesFromUpdate(PlayerUpdateInfo info)
	{
		foreach (var enemy in info.EnemyUpdates.Added)
		{
			var sprite = CreateEnemySprite(enemy);
			_enemyIdToSprite[enemy.Id] = sprite;
		}

		foreach (var enemy in info.EnemyUpdates.Removed)
		{
			if (_enemyIdToSprite.TryGetValue(enemy.Id, out EnemySprite enemySprite))
			{
				enemySprite.Kill();
				_enemyIdToSprite.Remove(enemy.Id);
			}
		}

		foreach (var towerShot in info.TowerShotUpdates.Added)
		{
			if (_enemyIdToSprite.TryGetValue(towerShot.TargetId, out EnemySprite enemySprite))
			{
				_tower.CreateTowerShotSprite(towerShot, enemySprite);
			}
		}

		foreach (var towerShot in info.TowerShotUpdates.Removed)
		{
			_tower.KillTowerShotSprite(towerShot.Id);
		}
	}
}