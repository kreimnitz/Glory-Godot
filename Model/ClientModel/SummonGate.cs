using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ProtoBuf;

[ProtoContract]
public class SummonGate : IUpdateFrom<SummonGate>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public List<Spawner> Spawners { get; } = new();

    public delegate void SpawnersAddedHandler(object sender, SpawnersAddedEventArgs e);
    public event SpawnersAddedHandler SpawnersAdded;

    public Spawner GetSpawnerForType(UnitType unitType)
    {
        return Spawners.FirstOrDefault(s => s.UnitType == unitType);
    }

    public void UpdateFrom(SummonGate other)
    {
        var spawnerUpdates = UpdateUtilites.UpdateMany(Spawners, other.Spawners);
        if (spawnerUpdates.Added.Any())
        {
            NotifySpawnersAdded(spawnerUpdates.Added);
        }
    }

    private void NotifySpawnersAdded(List<Spawner> added)
    {
        SpawnersAdded?.Invoke(this, new SpawnersAddedEventArgs() { Added = added });
    }
}

public class SpawnersAddedEventArgs
{
    public List<Spawner> Added { get; set; }
}