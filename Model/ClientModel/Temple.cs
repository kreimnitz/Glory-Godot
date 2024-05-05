using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class Temple : IUpdateFrom<Temple, PropertyUpdateInfo>
{
    public const int BuildDurationMs = 20000;
    public const int BuildCost = 400;
    public const int ConvertDurationMs = 20000;
    public const int ConvertCost = 200;

    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int FollowerCount { get; set; }

    [ProtoMember(3)]
    public bool IsActive { get; set; }

    [ProtoMember(4)]
    public Element Element { get; set; } = Element.None;

    [ProtoMember(5)]
    public int Position { get; set; }

    [ProtoMember(6)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    public PropertyUpdateInfo UpdateFrom(Temple other)
    {
        var updateInfo = new PropertyUpdateInfo();
        if (Id != other.Id)
        {
            Id = other.Id;
            updateInfo.Add(nameof(Id));
        }
        if (IsActive != other.IsActive)
        {
            IsActive = other.IsActive;
            updateInfo.Add(nameof(IsActive));
        }
        if (FollowerCount != other.FollowerCount)
        {
            FollowerCount = other.FollowerCount;
            updateInfo.Add(nameof(FollowerCount));
        }
        if (Element != other.Element)
        {
            Element = other.Element;
            updateInfo.Add(nameof(Element));
        }
        if (Position != other.Position)
        {
            Position = other.Position;
            updateInfo.Add(nameof(Position));
        }
        UpdateUtilites.UpdateMany<ProgressItem, DummyUpdateInfo>(TaskQueue, other.TaskQueue);
        return updateInfo;
    }
}

[Flags]
public enum Element
{
    None = 0,
    Fire = 1,
}