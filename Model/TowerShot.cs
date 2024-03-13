using System;

public class TowerShot
{
    private DateTime _creationTime;
    private int _durationMs;

    public int Id { get; set; }
    public double ProgressRatio { get; set; }
    public Tower Source { get; set; }
    public Enemy Target { get; set; }

    public TowerShot()
    {
        _creationTime = DateTime.UtcNow;
    }

    public void UpdateProgressRatio()
    {
        ProgressRatio = (DateTime.UtcNow - _creationTime).TotalMilliseconds / _durationMs;
    }
}