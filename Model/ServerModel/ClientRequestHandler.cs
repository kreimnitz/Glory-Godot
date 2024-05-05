using System.Linq;

public class ClientRequestHandler
{
    private ServerPlayer _serverPlayer;
    private TempleRequestHandler _templeRequestHandler;

    public ClientRequestHandler(ServerPlayer player)
    {
        _serverPlayer = player;
        _templeRequestHandler = new(player);
    }

    public void HandleTempleRequest(TempleActionRequestData data)
    {
        _templeRequestHandler.HandleRequest(data);
    }

    public void HandleSummonRequest(SummonRequestData data)
    {
        var spawner = _serverPlayer.ServerSummonGate.GetSpawnerForType(data.Type);
        var info = Enemies.TypeToInfo[data.Type];
        if (spawner == null || !spawner.DecrementQueue() || _serverPlayer.Player.Glory < info.GloryCost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= info.GloryCost;
        var enemy = new ServerEnemy(info, _serverPlayer.EnemyPath);
        _serverPlayer.Opponent.AddEnemy(enemy);
    }

    public void HandleBuildTempleRequest(BuildTempleRequestData data)
    {
        if (_serverPlayer.Player.Glory < Temple.BuildCost)
        {
            return;
        }

        _serverPlayer.Player.Glory -= Temple.BuildCost;
        _serverPlayer.BuildTemple(data.Position);
    }

    public void HandleEnemyTechRequest(EnemyTechRequestData data)
    {
        var enemyInfo = Enemies.TypeToInfo[data.EnemyType];
        var techInfo = enemyInfo.RequiredTech;
        var progressType = ProgressItemTypeHelpers.FromTech(techInfo.Tech);
        var alreadyQueued = _serverPlayer.Player.TaskQueue.Any(t => t.Type == progressType);
        if (_serverPlayer.Player.Glory < techInfo.GloryCost
            || _serverPlayer.Player.Tech.HasAllFlags(techInfo.Tech)
            || alreadyQueued)
        {
            return;
        }
        _serverPlayer.Player.Glory -= techInfo.GloryCost;
        _serverPlayer.QueueUnlockEnemyResearch(enemyInfo);
    }
}