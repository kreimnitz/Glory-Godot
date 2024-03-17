using System;

public class DelayedAction
{
    private int _durationMs;
    private DateTime _startTime;

    public Action Action { get; }

    public ProgressItem ToProgressItem()
    { 
        return new ProgressItem()
        {
            ProgressRatio = StaticUtilites.GetTimeProgressRatio(_startTime, _durationMs)
        };
    }

    public bool Ready => StaticUtilites.GetTimeProgressRatio(_startTime, _durationMs) > 1.0;

    public DelayedAction(Action action, int durationMs)
    {
        _startTime = DateTime.MaxValue;
        _durationMs = durationMs;
        Action = action;
    }

    public void Start()
    {
        if (_startTime == DateTime.MaxValue)
        {
            _startTime = DateTime.UtcNow;
        }
    }
}