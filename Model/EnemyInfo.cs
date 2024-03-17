using System;
using ProtoBuf;

[ProtoContract]
public class EnemyInfo
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    public void UpdateFrom(EnemyInfo other)
    {
        ProgressRatio = other.ProgressRatio;
    }
}