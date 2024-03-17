using System;

public class DelayedAction : ProgressItem
{
    private int _durationMs;
    private DateTime _creationTime;

    public Action Action { get; }

    public override double ProgressRatio
    { 
        get
        {
            return StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);
        }
    }

    public bool Ready => ProgressRatio > 1.0;

    public DelayedAction(Action action, int durationMs)
    {
        _creationTime = DateTime.UtcNow;
        _durationMs = durationMs;
        Action = action;
    }
}