using System.Linq;

public class ClientRequestHandler
{
    private ServerPlayer _serverPlayer;

    public ClientRequestHandler(ServerPlayer player)
    {
        _serverPlayer = player;
    }

    public void HandleAddFollowerRequest(TempleIndexData data)
    {
        if (_serverPlayer.Player.Glory < ServerTemple.FollowerCost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= ServerTemple.FollowerCost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueNewFollower();
    }

    public void HandleConvertToFireTempleRequest(TempleIndexData data)
    {
        if (_serverPlayer.Player.Glory < ServerTemple.BuildCost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= ServerTemple.BuildCost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueConvertToElement(Element.Fire);
    }

    public void HandleUnlockFireImpRequest()
    {
        var validTemples = _serverPlayer.Player.Temples.Where(t => t.IsActive && t.Element == Element.Fire);
        if (!validTemples.Any() || _serverPlayer.Player.Glory < Spawners.FireImpUnlockCost)
        {
            return;
        }

        var fireImpSpawner = Spawners.CreateFireImpSpawner();
        var delayedAction = new DelayedAction(
            ProgressItemType.UnlockFireImp,
            () => fireImpSpawner.Activate(),
            Spawners.FireImpUnlockDurationMs);
        _serverPlayer.InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleSpawnFireImpRequest(TempleIndexData data)
    {
        var impSpawner = _serverPlayer.ServerTemples[data.TempleIndex].GetSpawnerForType(EnemyType.FireImp);
        if (_serverPlayer.Player.Glory < EnemyUtilites.FireImpCost || !impSpawner.DecrementQueue())
        {
            return;
        }

        _serverPlayer.Player.Glory -= EnemyUtilites.FireImpCost;
        var imp = EnemyUtilites.CreateFireImp();
        _serverPlayer.Opponent.AddEnemy(imp);
    }
}