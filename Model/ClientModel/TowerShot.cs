using System;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(7, typeof(ServerTowerShot))]
public class TowerShot : IUpdateFrom<TowerShot>
{
    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public Guid TargetId { get; set; }

    public void UpdateFrom(TowerShot other)
    {
        Id = other.Id;
        ProgressRatio = other.ProgressRatio;
        TargetId = other.TargetId;
    }
}