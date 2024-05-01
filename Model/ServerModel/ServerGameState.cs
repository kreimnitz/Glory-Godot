using System.Timers;
using Utilities.Comms;

public class ServerGameState
{
    // shoot for server to update 120 times per second
    public const double LoopRateMs = 1000 / 120;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();

    private bool _solo;
    private ServerPlayer _player0;
    private ServerPlayer _player1;
    private ClientRequestHandler _player0RequestHandler;
    private ClientRequestHandler _player1RequestHandler;

    public ServerGameState(ServerMessageTransmitter serverMessenger, bool solo)
    {
        _solo = solo;
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);

        InitializePlayers();
    }

    private void InitializePlayers()
    {
        _player0 = new(0);
        _player1 = new(1);
        _player0.Opponent = _player1;
        _player1.Opponent = _player0;

        _player0RequestHandler = new(_player0);
        _player1RequestHandler = new(_player1);
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
        CheckGameEnd();
    }

    public void CheckGameEnd()
    {
        if (_player0.Player.HpCurrent <= 0 || _player1.Player.HpCurrent <= 0)
        {
            _loopTimer.Stop();
        }
    }

    private void DoChildrenLoops()
    {
        _player0.DoLoop();
        _player1.DoLoop();
    }

    private void SendGameStateMessage(ServerPlayer serverPlayer)
    {
        if (serverPlayer == null)
        {
            return;
        }
        var gameStateInfo = new GameStateInfo
        {
            PlayerInfo = serverPlayer.Player
        };

        var messageData = SerializationUtilities.ToByteArray(gameStateInfo);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, serverPlayer.PlayerNumber);
    }

    public void HandleClientRequest(ClientRequestType request, byte[] data, int playerId)
    {
        var handler = playerId == 0 ? _player0RequestHandler : _player1RequestHandler;
        switch (request)
        {
            case ClientRequestType.TempleRequest:
            {
                var templeRequestData = SerializationUtilities.FromByteArray<TempleRequestData>(data);
                handler.HandleTempleRequest(templeRequestData);
                break;
            }
            case ClientRequestType.PlayerRequest:
            {
                handler.HandleRecruitFollowerRequest();
                break;
            }
            case ClientRequestType.SummonRequest:
            {
                var summonRequestData = SerializationUtilities.FromByteArray<SummonRequestData>(data);
                handler.HandleSummonRequest(summonRequestData);
                break;
            }
            case ClientRequestType.DEBUG_SpawnEnemy:
            {
                var imp0 = new ServerEnemy(Enemies.FireImpInfo, _player0.EnemyPath);
                _player0.AddEnemy(imp0);

                var imp1 = new ServerEnemy(Enemies.FireImpInfo, _player1.EnemyPath);
                _player1.AddEnemy(imp1);
                break;
            }
            case ClientRequestType.DEBUG_ResetPlayers:
            {
                InitializePlayers();
                Start();
                break;
            }
            default:
                break;
        }
    }
}