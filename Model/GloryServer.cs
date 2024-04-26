using Utilities.Comms;

public class GloryServer : IClientMessageRecievedHandler
{
    private ServerGameState _gameState;
    private ServerMessageTransmitter _serverMessenger;

    public bool PlayersConnected => _serverMessenger.Connected;

    public GloryServer(bool solo)
    {
        _serverMessenger = new(this, solo);
        _gameState = new(_serverMessenger, solo);
    }

    public void HandleClientMessage(Message message, int playerId)
    {
        var request = (ClientRequestType)message.MessageTypeId;
        _gameState.HandleClientRequest(request, message.Data, playerId);
    }

    public void StartGame()
    {
        _gameState.Start();
    }

    public void ShutDown()
    {
        _serverMessenger.Close();
    }
}