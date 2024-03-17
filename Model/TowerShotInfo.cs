using System;
using ProtoBuf;

[ProtoContract]
public class TowerShotInfo
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public Guid TargetId { get; set; }

    public void UpdateFrom(TowerShotInfo other)
    {
        Id = other.Id;
        ProgressRatio = other.ProgressRatio;
        TargetId = other.TargetId;
    }
}