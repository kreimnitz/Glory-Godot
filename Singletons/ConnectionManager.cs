using Utilities.Comms;

public class ConnectionManager
{
    private IServerMessageReceivedHandler _serverMessageReceivedHandler;

    public static ConnectionManager Instance { get; private set; }
    public static void CreateSingleton(IServerMessageReceivedHandler handler)
    {
        if (Instance is null)
        {
            Instance = new ConnectionManager(handler);
        }
    }

    private ConnectionManager(IServerMessageReceivedHandler handler)
    {
        _serverMessageReceivedHandler = handler;
    }

    public GloryServer Server { get; private set; } = null;

    public ClientMessageTransmitter Client { get; private set; }

    public void StartGame()
    {
        Server?.StartGame();
    }

    public void CreateServer(bool solo)
    {
        if (Server is not null)
        {
            return;
        }
        Server = new GloryServer(solo);
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        if (Client is not null)
        {
            return;
        }
        Client = new(_serverMessageReceivedHandler);
    }
}