using System;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(7, typeof(ServerSpawner))]
public class Spawner : IUpdateFrom<Spawner>
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public virtual int QueueCount { get; protected set; }

    [ProtoMember(3)]
    public virtual int QueueMax { get; protected set; }

    [ProtoMember(4)]
    public EnemyType EnemyType { get; protected set; }

    public void UpdateFrom(Spawner other)
    {
        QueueCount = other.QueueCount;
        QueueMax = other.QueueMax;
        EnemyType = other.EnemyType;
    }
}