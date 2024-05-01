public class ServerTemple
{
    private ServerPlayer _player;

    public DelayedActionQueue InProgressQueue { get; } = new();

    public Temple Temple { get; set; }

    public ServerTemple(ServerPlayer player, Temple temple)
    {
        _player = player;
        Temple = temple;
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
            Enemies.FireImpInfo.UnlockDuration);
        InProgressQueue.Enqueue(delayedAction);
    }
}