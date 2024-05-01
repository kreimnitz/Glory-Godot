using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

[ProtoContract]
public class Temple : IUpdateFrom<Temple>, INotifyPropertyChanged
{
    public const int BuildDurationMs = 20000;
    public const int BuildCost = 400;
    public const int ConvertDurationMs = 20000;
    public const int ConvertCost = 200;

    private Element _element = Element.None;
    private bool _isActive = false;

    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int FollowerCount { get; set; }

    [ProtoMember(3)]
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            if (value != _isActive)
            {
                _isActive = value;
                NotifyPropertyChanged(nameof(IsActive));
            }
        }
    }

    [ProtoMember(4)]
    public Element Element
    {
        get { return _element; }
        set
        {
            if (value != _element)
            {
                _element = value;
                NotifyPropertyChanged(nameof(Element));
            }
        }
    }

    [ProtoMember(6)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public void UpdateFrom(Temple other)
    {
        Id = other.Id;
        IsActive = other.IsActive;
        FollowerCount = other.FollowerCount;
        Element = other.Element;

        UpdateUtilites.UpdateMany(TaskQueue, other.TaskQueue);
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public enum Element
{
    None,
    Fire
}