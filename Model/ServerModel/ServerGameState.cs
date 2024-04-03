using System.Timers;
using Utilities.Comms;

public class ServerGameState
{
    // shoot for server to update 120 times per second
    private const double LoopRateMs = 1000 / 120;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();

    private bool _solo;
    private ServerPlayer _player0 = new(0);
    private ServerPlayer _player1 = new(1);

    public ServerGameState(ServerMessageTransmitter serverMessenger, bool solo)
    {
        _solo = solo;
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);

        _player0.OtherPlayer = _player1;
        _player1.OtherPlayer = _player0;
    }

    public void Start()
    {
        _loopTimer.Start();
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
        var player = playerId == 0 ? _player0 : _player1;
        switch (request)
        {
            case ClientRequests.AddFollower:
                player.HandleAddFollowerRequest();
                break;
            case ClientRequests.AddFireTemple:
                player.HandleAddFireTempleRequest();
                break;
            case ClientRequests.AddVent:
                player.HandleAddVentSpawnerRequest();
                break;
            default:
                break;
        }
    }
}