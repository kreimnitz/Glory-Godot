using System;
using System.Collections;
using ProtoBuf;

[ProtoContract]
public class ProgressItem : IUpdateFrom<ProgressItem>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public virtual double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public ProgressItemType Type { get; set; }

    public void UpdateFrom(ProgressItem other)
    {
        ProgressRatio = other.ProgressRatio;
    }
}

public enum ProgressItemType
{
    Default = 0,
    RecruitingFollower = 1,
    ConvertToFireTemple = 2,
    UnlockFireImp = 3,
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