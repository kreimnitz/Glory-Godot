using System;
using ProtoBuf;

[ProtoContract]
public class TowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;

    public TowerShotInfo Info { get; set; } = new();

    public void DoLoop()
    {
        Info.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }

    public TowerShot()
    {
        _creationTime = DateTime.UtcNow;
    }

    public TowerShot(Enemy target)
    {
        _creationTime = DateTime.UtcNow;
        Info.TargetId = target.Info.Id;
    }
}