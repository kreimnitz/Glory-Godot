using System.Collections.Generic;
using System.Timers;
using Utilities.Comms;
using ProtoBuf;
using System.Linq;

public class ConcurrentGameState
{
    private const int LoopRateMs = 4;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();
    private Tower Tower { get; set; } =  new Tower();

    private Player _player = new();

    private List<Enemy> _enemies = new();

    private Timer _enemyTimer = new(5000);

    private List<TowerShot> _towerShots = new();

    public ConcurrentGameState(ServerMessageTransmitter serverMessenger)
    {
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);

        _enemyTimer.Elapsed += (s, a) => _actionQueue.Add(() => AddEnemy(CreateBasicEnemy()));
    }

    private Enemy CreateBasicEnemy()
    {
        var enemy = new Enemy();
        enemy.Info.HpMax = 10;
        enemy.Info.HpCurrent = 10;
        return enemy;
    }

    public void Start()
    {
        _loopTimer.Start();
        _enemyTimer.Start();
    }

    private void DoLoop(ElapsedEventArgs a)
    {
        _actionQueue.ExecuteActions();
        DoChildrenLoops();

        CheckLifetimes();
        CheckForNewShot();
        SendGameStateMessage();
    }

    private void DoChildrenLoops()
    {
        _player.DoLoop();
        foreach (var enemy in _enemies)
        {
            enemy.DoLoop();
        }
        foreach (var towerShot in _towerShots)
        {
            towerShot.DoLoop();
        }
    }

    private void SendGameStateMessage()
    {
        var gameStateInfo = new GameStateInfo
        {
            Player = _player.Info,
            Enemies = _enemies.Select(e => e.Info).ToList(),
            TowerShots = _towerShots.Select(t => t.Info).ToList()
        };

        var messageData = SerializationUtilities.ToByteArray(gameStateInfo);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, 0);
    }

    public void HandleClientRequest(ClientRequests request)
    {
        switch (request)
        {
            case ClientRequests.AddFollower:
                _player.HandleAddFollowerRequest();
                break;
            default:
                break;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void CheckForNewShot()
    {
        var shot = Tower.CheckForNewShot(_enemies);
        if (shot is not null)
        {
            _towerShots.Add(shot);
        }
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < _enemies.Count)
        {
            if (_enemies[0].IsDead())
            {
                _enemies.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }
        
        index = 0;
        while (index < _towerShots.Count)
        {
            if (_towerShots[0].IsComplete)
            {
                _towerShots.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }
    }
}