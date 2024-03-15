using System;
using ProtoBuf;

[ProtoContract]
public class TowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;

    [ProtoMember(1)]
    public Guid Id { get; private set; }

    [ProtoMember(2)]
    public double ProgressRatio { get; private set; }

    [ProtoMember(3)]
    public Guid TargetId { get; set; }

    public void UpdateProgressRatio()
    {
        ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }

    public TowerShot()
    {
        _creationTime = DateTime.UtcNow;
        Id = IdGenerator.Generate();
    }

    public TowerShot(Enemy target)
    {
        _creationTime = DateTime.UtcNow;
        TargetId = target.Id;
        Id = IdGenerator.Generate();
    }

    public void UpdateFrom(TowerShot other)
    {
        Id = other.Id;
        ProgressRatio = other.ProgressRatio;
        TargetId = other.TargetId;
    }
}