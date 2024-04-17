using System;
using ProtoBuf;

[ProtoContract]
public class ProgressItem : IUpdateFrom<ProgressItem>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public virtual double ProgressRatio { get; protected set; }

    [ProtoMember(3)]
    public ProgressItemType Type { get; private set; }

    public void UpdateFrom(ProgressItem other)
    {
        ProgressRatio = other.ProgressRatio;
    }
}

public enum ProgressItemType
{
    RecruitingFollower = 1,
    SummonElementalTemple = 2,
}