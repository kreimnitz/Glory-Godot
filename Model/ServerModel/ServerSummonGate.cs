using System.Linq;

public class ServerSummonGate
{
    private ServerPlayer _serverPlayer;
    public SummonGate SummonGate { get; }
    public DelayedActionQueue InProgressQueue { get; } = new();
    public SyncedList<ServerSpawner, Spawner> Spawners { get; }

    public ServerSummonGate(ServerPlayer player)
    {
        _serverPlayer = player;
        SummonGate = player.Player.SummonGate;
        Spawners = new(SummonGate.Spawners, serverSpawner => serverSpawner.Spawner);
    }

    private ServerSpawner GetSpawnerForType(UnitType unitType)
    {
        return Spawners.FirstOrDefault(s => s.Spawner.UnitType == unitType);
    }

    public bool DecrementUnitType(UnitType unitType)
    {
        var spawner = GetSpawnerForType(unitType);
        if (spawner == null)
        {
            return false;
        }
        return spawner.DecrementQueue();
    }

    public void CreateSpawner(EnemyInfo info)
    {
        var spawner = new ServerSpawner(info);
        Spawners.Add(spawner);
    }

    public void DoLoop()
    {
        InProgressQueue.ApplyNextActionIfReady();
        SummonGate.TaskQueue = InProgressQueue.ToProgressItemList();
    }

    public void QueueUnlockEnemyResearch(EnemyInfo enemyInfo)
    {
        var delayedAction = new DelayedAction(
            ProgressItemTypeHelpers.FromTech(enemyInfo.RequiredTech.Tech),
            () => UnlockEnemy(enemyInfo),
            enemyInfo.RequiredTech.ResearchDurationMs);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void UnlockEnemy(EnemyInfo enemyInfo)
    {
        _serverPlayer.Player.Tech.UpdateFrom(_serverPlayer.Player.Tech | enemyInfo.RequiredTech.Tech);
        CreateSpawner(enemyInfo);
    }
}