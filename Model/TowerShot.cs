using System;

public class TowerShot
{
    private DateTime _creationTime;
    private int _durationMs = 200;

    private bool _complete = false;
    public bool IsComplete
    { 
        get
        {
            if (_complete) { return true; }
            _complete = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs) > 1;
            return _complete;
        }
    }

    public TowerShotInfo Info { get; } = new();

    public TowerShot()
    {
        _creationTime = DateTime.UtcNow;
    }

    public TowerShot(Enemy target)
    {
        _creationTime = DateTime.UtcNow;
        Info.TargetId = target.Info.Id;
    }

    public void DoLoop()
    {
        Info.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
    }
}