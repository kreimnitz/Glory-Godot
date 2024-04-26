using System.Linq;

public class ServerTemple
{
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDurationMs = 10000;
    public const int BuildDurationMs = 20000;
    public const int BuildCost = 400;
    public const int ConvertDurationMs = 20000;
    public const int ConvertCost = 200;

    private ServerPlayer _player;

    public DelayedActionQueue InProgressQueue { get; } = new();

    public Temple Temple { get; set; } = new();

    public SyncedList<ServerSpawner, Spawner> ServerSpawners { get; }

    public ServerTemple(ServerPlayer player)
    {
        _player = player;
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

    public void QueueBuild()
    {
        var delayedAction = new DelayedAction(ProgressItemType.BuildTemple, () => Temple.IsActive = true, BuildDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void QueueConvertToElement(Element element)
    {
        var itemType = ProgressItemTypeHelpers.ConvertToElementProgressType(element);
        var delayedAction = new DelayedAction(itemType, () => Temple.Element = element, ConvertDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void QueueUnlockFlameImp()
    {
        var delayedAction = new DelayedAction(
            ProgressItemType.UnlockFireImp,
            () => _player.UnlockFlameImp(),
            Spawners.FireImpUnlockDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }
}