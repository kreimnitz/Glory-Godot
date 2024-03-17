using System.Timers;
using Utilities.Comms;
using SysTimer = System.Timers.Timer;

public class GloryServer : IClientMessageRecievedHandler
{
    private ConcurrentGameState _gameState;
    private ServerMessageTransmitter _serverMessenger;

    public GloryServer()
    {
        _serverMessenger = new(this);
        _gameState = new(_serverMessenger);
        _gameState.Start();
    }

    public void HandleClientMessage(Message message, int playerId)
    {
        var request = (ClientRequests)message.MessageTypeId;
        _gameState.HandleClientRequest(request);
    }
}