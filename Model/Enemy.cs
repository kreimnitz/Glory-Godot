using System;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(7, typeof(ServerEnemy))]
public class Enemy : IUpdateFrom<Enemy>
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public int HpMax { get; set; }

    [ProtoMember(4)]
    public int HpCurrent { get; set; }

    public void UpdateFrom(Enemy other)
    {
        ProgressRatio = other.ProgressRatio;
        HpMax = other.HpMax;
        HpCurrent = other.HpCurrent;
    }
}