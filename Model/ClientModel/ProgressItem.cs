using System;
using ProtoBuf;

[ProtoContract]
public class ProgressItem : IUpdateFrom<ProgressItem>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    public void UpdateFrom(ProgressItem other)
    {
        ProgressRatio = other.ProgressRatio;
    }
}