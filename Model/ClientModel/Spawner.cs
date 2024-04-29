using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class Spawner : IUpdateFrom<Spawner>, IProgressInfo, INotifyPropertyChanged
{
    private int _currentValue;
    private int _max;
    private UnitType _unitType;

    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int CurrentValue
    {
        get { return _currentValue; }
        set
        {
            if (value != _currentValue)
            {
                _currentValue = value;
                NotifyPropertyChanged(nameof(CurrentValue));
            }
        }
    }

    [ProtoMember(3)]
    public int Max
    {
        get { return _max; }
        set
        {
            if (value != _max)
            {
                _max = value;
                NotifyPropertyChanged(nameof(Max));
            }
        }
    }

    [ProtoMember(4)]
    public UnitType UnitType
    {
        get { return _unitType; }
        set
        {
            if (value != _unitType)
            {
                _unitType = value;
                NotifyPropertyChanged(nameof(UnitType));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(Spawner other)
    {
        CurrentValue = other.CurrentValue;
        Max = other.Max;
        UnitType = other.UnitType;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}