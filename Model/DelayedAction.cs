using System;

public class DelayedAction : ProgressItem
{
    private int _durationMs;
    private DateTime _startTime;

    public Action Action { get; }

    public override double ProgressRatio
    { 
        get => StaticUtilites.GetTimeProgressRatio(_startTime, _durationMs);
        protected set => base.ProgressRatio = value; 
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