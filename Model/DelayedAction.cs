using System;
using SysTimer = System.Timers.Timer;

public class DelayedAction
{
    private int _durationMs;
    private DateTime _creationTime;
    private SysTimer _timer;

    public double ProgressRatio => StaticUtilites.GetTimeProgressRatio(_creationTime, _durationMs);

    public DelayedAction(Action action, int durationMs)
    {
        _durationMs = durationMs;
        _timer = new SysTimer(_durationMs);
        _timer.AutoReset = false;
        _timer.Elapsed += (a, s) => action();
    }

    public void Start()
    {
        _timer.Start();
    }
}