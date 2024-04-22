using System;

public class DelayedAction
{
    private int _durationMs;
    private DateTime _startTime;
    private ProgressItem _progressItem;


    public Action Action { get; }

    public bool Ready => StaticUtilites.GetTimeProgressRatio(_startTime, _durationMs) > 1.0;

    public DelayedAction()
    {
    }

    public DelayedAction(ProgressItemType type, Action action, int durationMs)
    {
        _progressItem = new();
        _progressItem.Type = type;
        _startTime = DateTime.MaxValue;
        _durationMs = durationMs;
        Action = action;
    }

    public ProgressItem GetProgressItem()
    {
        _progressItem.ProgressRatio = StaticUtilites.GetTimeProgressRatio(_startTime, _durationMs);
        return _progressItem;
    }

    public void Start()
    {
        if (_startTime == DateTime.MaxValue)
        {
            _startTime = DateTime.UtcNow;
        }
    }
}