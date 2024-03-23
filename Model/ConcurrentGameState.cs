using System.Collections.Generic;
using System.Timers;
using Utilities.Comms;
using ProtoBuf;
using System.Linq;

public class ConcurrentGameState
{
    // shoot for server to update 120 times per second
    private const double LoopRateMs = 1000 / 120;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();

    private Player _player1 = new();
    private Player _player2 = new();

    private Timer _enemyTimer = new(5000);

    public ConcurrentGameState(ServerMessageTransmitter serverMessenger)
    {
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);

        _enemyTimer.Elapsed += (s, a) => _actionQueue.Add(() => _player1.AddEnemy(CreateBasicEnemy()));
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
        SendGameStateMessage();
    }

    private void DoChildrenLoops()
    {
        _player1.DoLoop();
        _player2.DoLoop();
    }

    private void SendGameStateMessage()
    {
        var gameStateInfo = new GameStateInfo
        {
            Player1Info = _player1.Info,
        };

        var messageData = SerializationUtilities.ToByteArray(gameStateInfo);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, 0);
        _serverMessenger.SendMessage(message, 1);
    }

    public void HandleClientRequest(ClientRequests request)
    {
        switch (request)
        {
            case ClientRequests.AddFollower:
                _player1.HandleAddFollowerRequest();
                break;
            default:
                break;
        }
    }
}