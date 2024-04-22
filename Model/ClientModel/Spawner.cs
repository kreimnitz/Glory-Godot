using System;
using ProtoBuf;

[ProtoContract]
public class Spawner : IUpdateFrom<Spawner>, IProgressInfo
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public virtual int CurrentValue { get; set; }

    [ProtoMember(3)]
    public virtual int Max { get; set; }

    [ProtoMember(4)]
    public EnemyType EnemyType { get; set; }

    public void UpdateFrom(Spawner other)
    {
        CurrentValue = other.CurrentValue;
        Max = other.Max;
        EnemyType = other.EnemyType;
    }
}