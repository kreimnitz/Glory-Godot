using System.Linq;

public class ServerSummonGate
{
    public SummonGate SummonGate { get; }
    public SyncedList<ServerSpawner, Spawner> Spawners { get; }

    public ServerSummonGate(SummonGate summonGate)
    {
        SummonGate = summonGate;
        Spawners = new(SummonGate.Spawners, serverSpawner => serverSpawner.Spawner);
    }

    public ServerSpawner GetSpawnerForType(UnitType unitType)
    {
        return Spawners.FirstOrDefault(s => s.Spawner.UnitType == unitType);
    }

    public void CreateSpawner(EnemyInfo info)
    {
        var spawner = new ServerSpawner(info);
        Spawners.Add(spawner);
    }
}