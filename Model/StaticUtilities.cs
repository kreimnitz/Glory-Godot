using System;

public static class StaticUtilites
{
    public static double GetTimeProgressRatio(DateTime creationTime, int durationMs)
    {
        return (DateTime.UtcNow - creationTime).TotalMilliseconds / durationMs;
    }
}