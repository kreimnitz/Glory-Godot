using System.Timers;
using Utilities.Comms;

public class ConcurrentGameState
{
    // shoot for server to update 120 times per second
    private const double LoopRateMs = 1000 / 120;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();

    private bool _solo;
    private ServerPlayer _player0 = new(0);
    private ServerPlayer _player1 = new(1);

    private Timer _enemyTimer = new(5000);

    public ConcurrentGameState(ServerMessageTransmitter serverMessenger, bool solo)
    {
        _solo = solo;
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);

        _enemyTimer.Elapsed += (s, a) => _actionQueue.Add(() => _player0.AddEnemy(CreateBasicEnemy()));
    }

    private ServerEnemy CreateBasicEnemy()
    {
        var enemy = new ServerEnemy();
        enemy.HpMax = 10;
        enemy.HpCurrent = 10;
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
        SendGameStateMessage(_player0);
        if (!_solo)
        {
            SendGameStateMessage(_player1);
        }
    }

    private void DoChildrenLoops()
    {
        _player0.DoLoop();
        _player1.DoLoop();
    }

    private void SendGameStateMessage(ServerPlayer player)
    {
        if (player == null)
        {
            return;
        }
        var gameStateInfo = new GameStateInfo
        {
            PlayerInfo = player
        };

        var messageData = SerializationUtilities.ToByteArray(gameStateInfo);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, player.PlayerNumber);
    }

    public void HandleClientRequest(ClientRequests request, int playerId)
    {
        switch (request)
        {
            case ClientRequests.AddFollower:
                if (playerId == 0)
                {
                    _player0.HandleAddFollowerRequest();
                }
                else
                {
                    _player1.HandleAddFollowerRequest();
                }
                break;
            default:
                break;
        }
    }
}