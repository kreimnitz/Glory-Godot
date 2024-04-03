using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerTemple))]
public class Temple : IUpdateFrom<Temple>
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public string Name { get; protected set; }

    [ProtoMember(3)]
    public bool IsActive { get; set;}

    [ProtoMember(4)]
    public List<Spawner> Spawners { get; set; } = new();

    public void UpdateFrom(Temple other)
    {
        IsActive = other.IsActive;
        UpdateUtilites.UpdateMany(Spawners, other.Spawners);
    }
}