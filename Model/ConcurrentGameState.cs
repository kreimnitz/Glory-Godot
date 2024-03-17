using System.Collections.Generic;
using System.Timers;
using Utilities.Comms;

public class ConcurrentGameState
{
    private const int LoopRateMs = 4;
    private Timer _loopTimer = new(LoopRateMs);
    private ServerMessageTransmitter _serverMessenger;
    private ActionQueue _actionQueue = new();
    private Tower Tower { get; set; } =  new Tower();

    private Player _player = new();

    private List<Enemy> _enemies = new();

    private List<TowerShot> _towerShots = new();

    public ConcurrentGameState()
    {
    }

    public ConcurrentGameState(ServerMessageTransmitter serverMessenger)
    {
        _serverMessenger = serverMessenger;
        _loopTimer.Elapsed += (s, a) => DoLoop(a);
    }

    public void Start()
    {
        _loopTimer.Start();
    }

    private void DoLoop(ElapsedEventArgs a)
    {
        _actionQueue.ExecuteActions();
        UpdateProgress();
        CheckLifetimes();
        CheckForNewShot();
        SendGameStateMessage();
    }

    private void SendGameStateMessage()
    {
        var messageData = SerializationUtilities.ToByteArray(this);
        var message = new Message(0, messageData);
        _serverMessenger.SendMessage(message, 0);
    }

    public void HandleClientRequest(ClientRequests request)
    {
        switch (request)
        {
            case ClientRequests.AddFollower:
                _player.HandleAddFollowerRequest();
                break;
            default:
                break;
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    public void UpdateProgress()
    {
        foreach (var enemy in _enemies)
        {
            enemy.DoLoop();
        }
        foreach (var towerShot in _towerShots)
        {
            towerShot.DoLoop();
        }
    }

    public void CheckForNewShot()
    {
        var shot = Tower.CheckForNewShot(_enemies);
        if (shot is not null)
        {
            _towerShots.Add(shot);
        }
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < _enemies.Count)
        {
            if (_enemies[0].Info.ProgressRatio >= 1)
            {
                _enemies.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }

        index = 0;
        while (index < _towerShots.Count)
        {
            if (_towerShots[0].Info.ProgressRatio >= 1)
            {
                _towerShots.RemoveAt(0);
            }
            else
            {
                index++;
            }
        }
    }
}