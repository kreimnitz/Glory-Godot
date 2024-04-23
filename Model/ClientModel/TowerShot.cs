using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerTowerShot))]
public class TowerShot : IUpdateFrom<TowerShot>, INotifyPropertyChanged
{
    private double _progressRatio;
    private Guid _targetId;

    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio
    {
        get { return _progressRatio; }
        set
        {
            if (value != _progressRatio)
            {
                _progressRatio = value;
                NotifyPropertyChanged(nameof(ProgressRatio));
            }
        }
    }

    [ProtoMember(3)]
    public Guid TargetId
    {
        get { return _targetId; }
        set
        {
            if (value != _targetId)
            {
                _targetId = value;
                NotifyPropertyChanged(nameof(TargetId));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(TowerShot other)
    {
        Id = other.Id;
        ProgressRatio = other.ProgressRatio;
        TargetId = other.TargetId;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}