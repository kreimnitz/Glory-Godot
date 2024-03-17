using System.Timers;
using Utilities.Comms;
using SysTimer = System.Timers.Timer;

public class GloryServer : IClientMessageRecievedHandler
{
    private ConcurrentGameState _gameState = new();
    private ServerMessageTransmitter _serverMessenger;
    private SysTimer _enemyTimer = new(3000);

    public GloryServer()
    {
        _serverMessenger = new(this);
        _serverMessenger.WaitForReady.ContinueWith((t) => _gameState.Start());
    }

    public void HandleClientMessage(Message message, int playerId)
    {
        var request = (ClientRequests)message.MessageTypeId;
        _gameState.HandleClientRequest(request);
    }
}