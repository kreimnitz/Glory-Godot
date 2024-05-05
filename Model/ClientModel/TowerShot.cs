using System;
using System.ComponentModel;
using Godot;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerTowerShot))]
public class TowerShot : IUpdateFrom<TowerShot, PropertyUpdateInfo>
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public float PositionX { get; set; }

    [ProtoMember(3)]
    public float PositionY { get; set; }

    [ProtoMember(4)]
    public Guid TargetId { get; set; }

    public PropertyUpdateInfo UpdateFrom(TowerShot other)
    {
        Id = other.Id;
        PositionX = other.PositionX;
        PositionY = other.PositionY;
        TargetId = other.TargetId;
        return new PropertyUpdateInfo();
    }
}