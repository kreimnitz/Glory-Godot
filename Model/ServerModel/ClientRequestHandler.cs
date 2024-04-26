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
        if (_serverPlayer.Player.Glory < Temple.FollowerCost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.FollowerCost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueNewFollower();
    }

    public void HandleBuildTempleRequest(TempleIndexData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.BuildTemple);
        if (_serverPlayer.Player.Glory < Temple.BuildCost || serverTemple.Temple.IsActive || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.BuildCost;
        serverTemple.QueueBuild();
    }

    public void HandleConvertToFireTempleRequest(TempleIndexData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (_serverPlayer.Player.Glory < Temple.ConvertCost || serverTemple.Temple.Element != Element.None || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.ConvertCost;
        serverTemple.QueueConvertToElement(Element.Fire);
    }

    public void HandleUnlockFireImpRequest(TempleIndexData data)
    {
        var serverTemple = _serverPlayer.ServerTemples[data.TempleIndex];
        var alreadyQueued = serverTemple.Temple.TaskQueue.Any(t => t.Type == ProgressItemType.ConvertToFireTemple);
        if (serverTemple.Temple.Element != Element.Fire 
            || _serverPlayer.Player.Glory < Spawners.FireImpUnlockCost
            || _serverPlayer.Player.Tech.FireTech.HasFlag(FireTech.FlameImp)
            || alreadyQueued)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Spawners.FireImpUnlockCost;
        _serverPlayer.ServerTemples[data.TempleIndex].QueueUnlockFlameImp();
    }

    public void HandleSpawnFireImpRequest(TempleIndexData data)
    {
        var impSpawner = _serverPlayer.ServerTemples[data.TempleIndex].GetSpawnerForType(EnemyType.FireImp);
        if (impSpawner is null)
        {
            return;
        }

        if (_serverPlayer.Player.Glory < EnemyUtilites.FireImpCost || !impSpawner.DecrementQueue())
        {
            return;
        }

        _serverPlayer.Player.Glory -= EnemyUtilites.FireImpCost;
        var imp = EnemyUtilites.CreateFireImp(_serverPlayer.EnemyPath);
        _serverPlayer.Opponent.AddEnemy(imp);
    }
}