using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class Spawner : IUpdateFrom<Spawner>, IProgressInfo, INotifyPropertyChanged
{
    private int _currentValue;
    private int _max;
    private EnemyType _enemyType;

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
    public EnemyType EnemyType
    {
        get { return _enemyType; }
        set
        {
            if (value != _enemyType)
            {
                _enemyType = value;
                NotifyPropertyChanged(nameof(EnemyType));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(Spawner other)
    {
        CurrentValue = other.CurrentValue;
        Max = other.Max;
        EnemyType = other.EnemyType;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}