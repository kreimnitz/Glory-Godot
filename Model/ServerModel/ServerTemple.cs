using System.Linq;

public class ServerTemple
{
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDurationMs = 10000;
    public const int CreateDurationMs = 20000;
    public const int BuildCost = 200;
    public const int ConvertDurationMs = 20000;
    public const int ConvertCost = 200;

    public DelayedActionQueue InProgressQueue { get; } = new();

    public Temple Temple { get; set; } = new();

    public SyncedList<ServerSpawner, Spawner> ServerSpawners { get; }

    public ServerTemple()
    {
        ServerSpawners = new(Temple.Spawners, serverSpawner => serverSpawner.Spawner);
    }

    public ServerSpawner GetSpawnerForType(EnemyType enemyType)
    {
        if (!Temple.IsActive)
        {
            return null;
        }
        return ServerSpawners.FirstOrDefault(s => s.Spawner.EnemyType == enemyType);
    }

    public void DoLoop()
    {
        InProgressQueue.ApplyNextActionIfReady();
        Temple.TaskQueue = InProgressQueue.ToProgressItemList();
    }

    public void QueueNewFollower()
    {
        var delayedAction = new DelayedAction(ProgressItemType.RecruitingFollower, () => Temple.FollowerCount++, FollowerTrainDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void QueueConvertToElement(Element element)
    {
        var itemType = ProgressItemTypeHelpers.ConvertToElementProgressType(element);
        var delayedAction = new DelayedAction(itemType, () => Temple.Element = element, ConvertDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }
}