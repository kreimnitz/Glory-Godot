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

    public void SendTempleRequest(TempleActionRequest request, int templeIndex)
    {
        var data = new TempleActionRequestData() { Request = request, TempleIndex = templeIndex };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.TempleActionRequest, serialized);
    }

    public void SendTempleTechRequest(PlayerTech tech, int templeIndex)
    {
        var data = new TempleTechRequestData() { Tech = tech, TempleIndex = templeIndex };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.TempleTechRequest, serialized);
    }

    public void SendSummonRequest(UnitType type)
    {
        var data = new UnitTypeData() { Type = type };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.SummonRequest, serialized);
    }

    public void SendEnemyTechRequest(UnitType type)
    {
        var data = new UnitTypeData() { Type = type };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.EnemyTechRequest, serialized);
    }

    public void SendBuildTempleRequest(int position)
    {
        var data = new BuildTempleRequestData() { Position = position };
        var serialized = SerializationUtilities.ToByteArray(data);
        SendMessage(ClientRequestType.BuildTemple, serialized);
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