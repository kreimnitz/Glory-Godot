using System;
using System.ComponentModel;
using Godot;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerTowerShot))]
public class TowerShot : IUpdateFrom<TowerShot>, INotifyPropertyChanged
{
    private float _positionX;
    private float _positionY;
    private Guid _targetId;

    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public float PositionX
    {
        get { return _positionX; }
        set
        {
            if (!_positionX.Equals(value))
            {
                _positionX = value;
                NotifyPropertyChanged(nameof(PositionX));
            }
        }
    }

    [ProtoMember(3)]
    public float PositionY
    {
        get { return _positionY; }
        set
        {
            if (!_positionY.Equals(value))
            {
                _positionY = value;
                NotifyPropertyChanged(nameof(PositionY));
            }
        }
    }

    [ProtoMember(4)]
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
        PositionX = other.PositionX;
        PositionY = other.PositionY;
        TargetId = other.TargetId;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}