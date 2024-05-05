public class ServerTemple
{
    private ServerPlayer _player;

    public DelayedActionQueue InProgressQueue { get; } = new();

    public Temple Temple { get; set; } = new();

    public ServerTemple(ServerPlayer player, int position)
    {
        _player = player;
        Temple = new Temple();
        Temple.Position = position;
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
        var itemType = ProgressItemTypeHelpers.FromElement(element);
        var delayedAction = new DelayedAction(itemType, () => ConvertToElement(element), Temple.ConvertDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    private void ConvertToElement(Element element)
    {
        Temple.Element |= element;
        _player.Player.Element |= element;
    }
}