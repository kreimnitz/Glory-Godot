using System.Collections.Generic;
using System.Linq;

public class ServerTemple : Temple
{
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDurationMs = 10000;
    public const int CreateDurationMs = 20000;
    public const int Cost = 150;

    public DelayedActionQueue InProgressQueue { get; } = new();

    public override List<ProgressItem> TaskQueue
    {
        get => InProgressQueue.ToProgressItemList();
        protected set => base.TaskQueue = value;
    }

    public void DoLoop()
    {
        InProgressQueue.ApplyNextActionIfReady();
    }

    public void QueueNewFollower()
    {
        var delayedAction = new DelayedAction(() => FollowerCount++, FollowerTrainDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public ServerSpawner GetSpawnerForType(EnemyType enemyType)
    {
        if (!IsActive)
        {
            return null;
        }
        return (ServerSpawner)Spawners.FirstOrDefault(s => s.EnemyType == enemyType);
    }
}