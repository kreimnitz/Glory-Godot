using System;
using System.Runtime.CompilerServices;
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

    public void SendTempleIndexMessage(ClientRequestType requestType, int templeIndex)
    {
        var data = new TempleIndexData() { TempleIndex = templeIndex };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(requestType, serialized);
    }

    private void SendMessage(ClientRequestType request)
    {
        SendMessage(request, Array.Empty<byte>());
    }

    public void SendMessage(ClientRequestType request, byte[] data)
    {
        var message = new Message((int)request, data);
        ConnectionManager.Instance.Client.SendMessage(message);
    }
}