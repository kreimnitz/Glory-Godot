using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerTemple))]
public class Temple : IUpdateFrom<Temple>
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int FollowerCount { get; set; }

    [ProtoMember(3)]
    public bool IsActive { get; set; } = false;

    [ProtoMember(4)]
    public List<Spawner> Spawners { get; set; } = new();

    [ProtoMember(5)]
    public Element Element { get; set; } = Element.None;

    [ProtoMember(6)]
    public virtual List<ProgressItem> TaskQueue { get; protected set; } = new();

    public void UpdateFrom(Temple other)
    {
        IsActive = other.IsActive;
        FollowerCount = other.FollowerCount;
        UpdateUtilites.UpdateMany(Spawners, other.Spawners);
    }
}

public enum Element
{
    None,
    Fire
}