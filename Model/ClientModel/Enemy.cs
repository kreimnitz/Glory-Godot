using System;
using ProtoBuf;

[ProtoContract]
public class Enemy : IUpdateFrom<Enemy, PropertyUpdateInfo>, IProgressInfo
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public float Progress { get; set; }

    [ProtoMember(3)]
    public int HpMax { get; set; }

    [ProtoMember(4)]
    public int HpCurrent { get; set; }

    [ProtoMember(5)]
    public UnitType Type { get; set; } = UnitType.Default;

    public int CurrentValue => HpCurrent;

    public int Max => HpMax;

    public PropertyUpdateInfo UpdateFrom(Enemy other)
    {
        Id = other.Id;
        Progress = other.Progress;
        HpMax = other.HpMax;
        HpCurrent = other.HpCurrent;
        Type = other.Type;
        return new PropertyUpdateInfo();
    }
}