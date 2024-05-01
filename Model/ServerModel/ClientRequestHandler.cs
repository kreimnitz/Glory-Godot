public class ClientRequestHandler
{
    private ServerPlayer _serverPlayer;
    private TempleRequestHandler _templeRequestHandler;

    public ClientRequestHandler(ServerPlayer player)
    {
        _serverPlayer = player;
        _templeRequestHandler = new(player);
    }

    public void HandleTempleRequest(TempleRequestData data)
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
}