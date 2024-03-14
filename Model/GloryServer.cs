using System.Timers;
using Utilities.Comms;
using SysTimer = System.Timers.Timer;

public class GloryServer : IClientMessageRecievedHandler
{
    private ConcurrentGameState _gameState = new();
    private ServerMessageTransmitter _serverMessenger;
    private SysTimer _enemyTimer = new(3000);
    private SysTimer _incomeTimer = new(5000);

    private SysTimer _loopTimer = new(4);

    public GloryServer()
    {
        _serverMessenger = new(this);

        _enemyTimer.Elapsed += (s, a) => _gameState.AddEnemy(new Enemy());
        _enemyTimer.Start();

        _incomeTimer.Elapsed += (s , a) => _gameState.ApplyIncome();
        _incomeTimer.Start();

        _loopTimer.Elapsed += (s, a) => DoLoop(a);
        _loopTimer.Start();
    }

    private void SendGameStateMessage()
    {
        var messageData = SerializationUtilities.ToByteArray(_gameState);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, 0);
    }

    private void DoLoop(ElapsedEventArgs a)
    {
        _gameState.UpdateProgress();
        _gameState.CheckLifetimes();
        _gameState.CheckForNewShot();
        SendGameStateMessage();
    }

    public void HandleClientMessage(Message message, int playerId)
    {
        
    }
}
