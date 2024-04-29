using System.Linq;

public class ServerTemple
{
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
        var delayedAction = new DelayedAction(ProgressItemType.RecruitingFollower, () => Temple.FollowerCount++, Follower.TrainDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void QueueBuild()
    {
        var delayedAction = new DelayedAction(ProgressItemType.BuildTemple, () => Temple.IsActive = true, Temple.BuildDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void QueueConvertToElement(Element element)
    {
        var itemType = ProgressItemTypeHelpers.ConvertToElementProgressType(element);
        var delayedAction = new DelayedAction(itemType, () => Temple.Element = element, Temple.ConvertDurationMs);
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