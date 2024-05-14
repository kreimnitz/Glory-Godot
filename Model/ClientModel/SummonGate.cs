using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

[ProtoContract]
public class SummonGate : IUpdateFrom<SummonGate, SummonGateUpdateInfo>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public List<Spawner> Spawners { get; } = new();

    [ProtoMember(3)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    public Spawner GetSpawnerForType(UnitType unitType)
    {
        return Spawners.FirstOrDefault(s => s.UnitType == unitType);
    }

    public SummonGateUpdateInfo UpdateFrom(SummonGate other)
    {
        UpdateUtilites.UpdateMany<ProgressItem, DummyUpdateInfo>(TaskQueue, other.TaskQueue);
        var spawnerUpdates = UpdateUtilites.UpdateMany<Spawner, PropertyUpdateInfo>(Spawners, other.Spawners);
        return new SummonGateUpdateInfo() { SpawnerUpdates = spawnerUpdates };
    }
}

public class SummonGateUpdateInfo
{
    public ListUpdateInfo<Spawner, PropertyUpdateInfo> SpawnerUpdates { get; set; }
}