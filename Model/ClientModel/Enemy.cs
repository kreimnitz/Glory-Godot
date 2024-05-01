using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class Enemy : IUpdateFrom<Enemy>, INotifyPropertyChanged, IProgressInfo
{
    private float _progress;
    private int _hpMax;
    private int _hpCurrent;

    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public float Progress
    { 
        get { return _progress; }
        set 
        {
            if (value != _progress)
            {
                _progress = value;
                NotifyPropertyChanged(nameof(Progress));
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

    [ProtoMember(5)]
    public UnitType Type { get; set; } = UnitType.Default;

    public int CurrentValue => HpCurrent;

    public int Max => HpMax;

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(Enemy other)
    {
        Type = other.Type;
        Progress = other.Progress;
        HpMax = other.HpMax;
        HpCurrent = other.HpCurrent;
        return ;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}