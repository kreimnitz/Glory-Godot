using System;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class ProgressItem : IUpdateFrom<ProgressItem>, INotifyPropertyChanged
{
    private double _progressRatio;
    private ProgressItemType _type;

    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

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
    public ProgressItemType Type
    {
        get { return _type; }
        set
        {
            if (value != _type)
            {
                _type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(ProgressItem other)
    {
        ProgressRatio = other.ProgressRatio;
        Type = other.Type;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum ProgressItemType
{
    Default = 0,
    RecruitingFollower = 1,
    BuildTemple = 2,
    ConvertToFireTemple = 3,
    UnlockFireImp = 4,
}

public static class ProgressItemTypeHelpers
{
    public static ProgressItemType ConvertToElementProgressType(Element element)
    {
        switch (element)
        {
            case Element.Fire: return ProgressItemType.ConvertToFireTemple;
            default:
                return ProgressItemType.Default;
        }
    }
}