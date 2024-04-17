using System.Linq;

public class ClientRequestHandler
{
    private ServerPlayer _player;
    public ClientRequestHandler(ServerPlayer player)
    {
        _player = player;
    }

    public void HandleAddFollowerRequest(TempleIndexData data)
    {
        if (_player.Glory < ServerTemple.FollowerCost)
        {
            return;
        }

        _player.Glory -= ServerTemple.FollowerCost;
        var temple = (ServerTemple)_player.Temples[data.TempleIndex];
        temple.QueueNewFollower();
    }

    public void HandleConvertToFireTempleRequest(TempleIndexData data)
    {
        if (_player.Glory < ServerTemple.Cost)
        {
            return;
        }

        _player.Glory -= ServerTemple.Cost;

        var delayedAction = new DelayedAction(() => _player.Temples[data.TempleIndex].Element = Element.Fire, ServerTemple.CreateDurationMs);
        _player.InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleUnlockFireImpRequest()
    {
        var validTemples = _player.Temples.Where(t => t.IsActive && t.Element == Element.Fire);
        if (!validTemples.Any() || _player.Glory < Spawners.FireImpUnlockCost)
        {
            return;
        }

        var fireImpSpawner = Spawners.CreateFireImpSpawner();
        var delayedAction = new DelayedAction(() => fireImpSpawner.Activate(), Spawners.FireImpUnlockDurationMs);
        _player.InProgressQueue.Enqueue(delayedAction);
    }

    public void HandleSpawnFireImpRequest()
    {
        var impSpawner = ((ServerTemple)_player.Temples[0]).GetSpawnerForType(EnemyType.FireImp);
        if (_player.Glory < EnemyUtilites.FireImpCost || !impSpawner.DecrementQueue())
        {
            return;
        }

        _player.Glory -= EnemyUtilites.FireImpCost;
        var imp = EnemyUtilites.CreateFireImp();
        _player.Opponent.AddEnemy(imp);
    }
}