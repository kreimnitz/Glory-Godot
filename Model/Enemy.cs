using System;
using ProtoBuf;

[ProtoContract]
public class Enemy
{
    private int _durationMs = 5000;
    private DateTime _creationTime;

    [ProtoMember(1)]
    public Guid Id { get; }

    [ProtoMember(2)]
    public double ProgressRatio { get; private set; }

    public Enemy()
    {
        _creationTime = DateTime.UtcNow;
        Id = IdGenerator.Generate();
    }

    public void UpdateProgressRatio()
    {
        ProgressRatio = (DateTime.UtcNow - _creationTime).TotalMilliseconds / _durationMs;
    }

    public void UpdateFrom(Enemy other)
    {
        ProgressRatio = other.ProgressRatio;
    }
}