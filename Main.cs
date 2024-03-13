using Godot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utilities.Comms;
using SysTimer = System.Timers.Timer;

public partial class Main : Node, IClientMessageRecievedHandler, IServerMessageReceivedHandler
{
	private Player _serverPlayer = new();

	private ClientMessageTransmitter _client;

	private TowerSprite _tower;
	private Path2D _enemyPath;
	private TopBar _topBar;

	private Player _player1 = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_client = new(this);

		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");

		_topBar = GetNode<TopBar>("TopBar");
		_topBar.Player = _player1;
	}

	private Queue<EnemySprite> _hiddenEnemies = new Queue<EnemySprite>();
    private void CreateEnemy()
    {
		if (_hiddenEnemies.TryDequeue(out EnemySprite toRecycle))
		{
			toRecycle.Reset();
			return;
		}

		var enemy = EnemySprite.CreateEnemy(_enemyPath);
		enemy.Hidden += () => _hiddenEnemies.Enqueue(enemy);
		AddChild(enemy);
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
		FireTowerShot();
	}

	private void FireTowerShot()
	{
		var enemies = GetChildren().OfType<EnemySprite>().Where(e => e.Visible);
		if (enemies.Any())
		{
			_tower.FireAtEnemies(enemies);
		}
	}

    public void HandleClientMessage(Message message, int playerId)
    {

    }

    public void HandleServerMessage(Message message)
    {
		if (message.MessageTypeId == 0)
		{
			var player = SerializationUtilities.FromByteArray<Player>(message.Data);
			_actions.Enqueue(() => _player1.UpdateFrom(player));
		}
    }
}
