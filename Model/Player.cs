using System.Collections.Concurrent;
using System.Linq;
using System.Timers;

public class Player
{
    public const int StartingGlory = 0;
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDuration = 10000;
    public const int IncomeTimerMs = 1000;

    private ActionQueue _actionQueue = new();
    private Timer _incomeTimer = new(IncomeTimerMs);

    public DelayedActionQueue InProgressQueue { get; } = new();

    public PlayerInfo Info { get; } = new();

    public Player()
    {
        Info.Glory = StartingGlory;
        Info.FollowerCount = StartingFollowerCount;
        _incomeTimer.Elapsed += (s, a) => _actionQueue.Add(() => ApplyIncome());
        _incomeTimer.Start();
    }

    public void DoLoop()
    {
        _actionQueue.ExecuteActions();
        InProgressQueue.ApplyNextActionIfReady();
        Info.TaskQueue = InProgressQueue.Actions.Select(da => da.ToProgressItem()).ToList();
    }

    private void ApplyIncome()
    {
        Info.Glory += Info.FollowerCount * IncomePerFollower;
    }

    public void HandleAddFollowerRequest()
    {
        if (Info.Glory < FollowerCost)
        {
            return;
        }

        Info.Glory -= FollowerCost;
        var delayedAction = new DelayedAction(() => Info.FollowerCount++, FollowerTrainDuration);
        InProgressQueue.Enqueue(delayedAction);
    }
}