using System.Collections.Concurrent;

public class Player
{
    public const int StartingGlory = 0;
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDuration = 10000;
    public const int IncomeTimerMs = 1000;

    public int Glory { get; set; } = StartingGlory;
    public int FollowerCount { get; set; } = StartingFollowerCount;
    public ConcurrentQueue<DelayedAction> InProgressQueue { get; } = new();

    public void DoLoop()
    {
        ApplyIncome();
        if (InProgressQueue.TryPeek(out DelayedAction delayedAction) && delayedAction.Ready)
        {
            if (InProgressQueue.TryDequeue(out delayedAction))
            {
                delayedAction.Action();
            }
        }
    }

    private void ApplyIncome()
    {
        Glory += FollowerCount * IncomePerFollower;
    }

    public void HandleAddFollowerRequest()
    {
        if (Glory < FollowerCost)
        {
            return;
        }

        Glory -= FollowerCost;
        var delayedAction = new DelayedAction(() => FollowerCount++, FollowerTrainDuration);
        InProgressQueue.Enqueue(delayedAction);
    }
}