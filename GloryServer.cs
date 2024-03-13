using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utilities.Comms;
using SysTimer = System.Timers.Timer;

public class GloryServer : IClientMessageRecievedHandler
{
    private Player _player;
    private ServerMessageTransmitter _serverMessenger;
    private SysTimer _enemyTimer = new(3000);
    private SysTimer _incomeTimer = new(5000);

    public GloryServer(IClientMessageRecievedHandler client)
    {
        _serverMessenger = new(client);

        _enemyTimer.Elapsed += (s, a) => CreateEnemeyAndSendServerMessage();
        _enemyTimer.Start();

        _incomeTimer.Elapsed += (s, a) => ApplyIncomeAndSendServerMessage();
        _incomeTimer.Start();
    }

    private void ApplyIncomeAndSendServerMessage()
    {
        _player.ApplyIncome();
        var messageData = SerializationUtilities.ToByteArray(_player);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, 0);
    }

    private void CreateEnemeyAndSendServerMessage()
    {

    }

    public void HandleClientMessage(Message message, int playerId)
    {
        
    }
}
