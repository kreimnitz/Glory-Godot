using System;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(7, typeof(ServerSpawner))]
public class Spawner : IUpdateFrom<Spawner>, IProgressInfo
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public virtual int CurrentValue { get; protected set; }

    [ProtoMember(3)]
    public virtual int Max { get; protected set; }

    [ProtoMember(4)]
    public EnemyType EnemyType { get; protected set; }

    public void UpdateFrom(Spawner other)
    {
        CurrentValue = other.CurrentValue;
        Max = other.Max;
        EnemyType = other.EnemyType;
    }
}