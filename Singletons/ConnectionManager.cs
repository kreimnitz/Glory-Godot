using System.Net;
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

    public bool Connected => (Server?.PlayersConnected ?? true) && (Client?.Connected ?? false);

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
        ConnectToServer(null);
    }

    public void ConnectToServer(string ipString)
    {
        if (Client is not null)
        {
            return;
        }
        if (ipString is null)
        {
            Client = new(_serverMessageReceivedHandler);
            return;
        }
        try
        {
            var ipAddress = IPAddress.Parse(ipString);
            Client = new(_serverMessageReceivedHandler, ipAddress);
        }
        catch
        {
            // do error handling
        }
    }

    public void CloseConnections()
    {
        Client?.Close();
        Server?.ShutDown();
    }
}