using System;
using ProtoBuf;

[ProtoContract]
public class EnemyInfo : IUpdateFrom<EnemyInfo>
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public int HpMax { get; set; }

    [ProtoMember(4)]
    public int HpCurrent { get; set; }

    public void UpdateFrom(EnemyInfo other)
    {
        ProgressRatio = other.ProgressRatio;
        HpMax = other.HpMax;
        HpCurrent = other.HpCurrent;
    }
}