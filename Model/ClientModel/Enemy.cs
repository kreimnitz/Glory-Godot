using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class Enemy : IUpdateFrom<Enemy>, INotifyPropertyChanged
{
    private double _progressRatio;
    private int _hpMax;
    private int _hpCurrent;

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
    public int HpMax
    {
        get { return _hpMax; }
        set
        {
            if (value != _hpMax)
            {
                _hpMax = value;
                NotifyPropertyChanged(nameof(HpMax));
            }
        }
    }

    [ProtoMember(4)]
    public int HpCurrent
    {
        get { return _hpCurrent; }
        set
        {
            if (value != _hpCurrent)
            {
                _hpCurrent = value;
                NotifyPropertyChanged(nameof(HpCurrent));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(Enemy other)
    {
        ProgressRatio = other.ProgressRatio;
        HpMax = other.HpMax;
        HpCurrent = other.HpCurrent;
        return ;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}