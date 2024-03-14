using Godot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Utilities.Comms;

public partial class Main : Node, IServerMessageReceivedHandler
{
	private GloryServer _server;
	private ClientMessageTransmitter _client;

	private TowerSprite _tower;
	private Path2D _enemyPath;
	private TopBar _topBar;

	private ClientGameState _gameState;

	private Dictionary<Guid, EnemySprite> _enemyIdToSprite = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_server = new GloryServer();
		_gameState = new();
		_client = new(this);

		_tower = GetNode<TowerSprite>("TowerSprite");
		_enemyPath = GetNode<Path2D>("EnemyPath");

		_topBar = GetNode<TopBar>("TopBar");
		_topBar.Player = _gameState.Player;
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
			var serverGameState = SerializationUtilities.FromByteArray<ConcurrentGameState>(message.Data);
			var updateInfo = _gameState.UpdateFrom(serverGameState);
			_actions.Enqueue(() => CreateSpritesFromUpdate(updateInfo));
		}
    }

	private void CreateSpritesFromUpdate(GameStateUpdateInfo info)
	{
		foreach (var enemy in info.NewEnemies)
		{
			var sprite = CreateEnemySprite(enemy);
			_enemyIdToSprite[enemy.Id] = sprite;
		}

		foreach (var enemy in info.RemovedEnemies)
		{
			if (_enemyIdToSprite.TryGetValue(enemy.Id, out EnemySprite enemySprite))
			{
				enemySprite.Kill();
				_enemyIdToSprite.Remove(enemy.Id);
			}
		}

		foreach (var towerShot in info.NewTowerShots)
		{
			if (_enemyIdToSprite.TryGetValue(towerShot.TargetId, out EnemySprite enemySprite))
			{
				_tower.CreateTowerShotSprite(towerShot, enemySprite);
			}
		}

		foreach (var towerShot in info.RemovedTowerShots)
		{
			_tower.KillTowerShotSprite(towerShot.Id);
		}
	}
}
