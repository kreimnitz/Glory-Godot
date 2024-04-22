using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

[ProtoContract]
public class Temple : IUpdateFrom<Temple>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int FollowerCount { get; set; }

    [ProtoMember(3)]
    public bool IsActive { get; set; } = false;

    [ProtoMember(4)]
    public List<Spawner> Spawners { get; set; } = new();

    [ProtoMember(5)]
    public Element Element { get; set; } = Element.None;

    [ProtoMember(6)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    public Spawner GetSpawnerForType(EnemyType enemyType)
    {
        if (!IsActive)
        {
            return null;
        }
        return Spawners.FirstOrDefault(s => s.EnemyType == enemyType);
    }

    public void UpdateFrom(Temple other)
    {
        Id = other.Id;
        IsActive = other.IsActive;
        FollowerCount = other.FollowerCount;
        Element = other.Element;
        
        UpdateUtilites.UpdateMany(Spawners, other.Spawners);
        UpdateUtilites.UpdateMany(TaskQueue, other.TaskQueue);
    }
}

public enum Element
{
    None,
    Fire
}