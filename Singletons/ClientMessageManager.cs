using System;
using Utilities.Comms;

public class ClientMessageManager
{
    public static ClientMessageManager Instance { get; private set; }
    public static void CreateSingleton()
    {
        if (Instance is null)
        {
            Instance = new ClientMessageManager();
        }
    }

    private ClientMessageManager()
    {
    }

    public void SendTempleRequest(TempleRequest request, int templeIndex)
    {
        var data = new TempleRequestData() { Request = request, TempleIndex = templeIndex };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.TempleRequest, serialized);
    }

    public void SendMessage(ClientRequestType request)
    {
        SendMessage(request, Array.Empty<byte>());
    }

    public void SendMessage(ClientRequestType request, byte[] data)
    {
        var message = new Message((int)request, data);
        ConnectionManager.Instance.Client.TrySendMessage(message);
    }
}