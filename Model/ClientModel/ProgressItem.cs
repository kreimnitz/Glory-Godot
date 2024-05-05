using System;
using ProtoBuf;

[ProtoContract]
public class ProgressItem : IUpdateFrom<ProgressItem, DummyUpdateInfo>
{
    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public double ProgressRatio { get; set; }

    [ProtoMember(3)]
    public ProgressItemType Type { get; set; }

    public DummyUpdateInfo UpdateFrom(ProgressItem other)
    {
        ProgressRatio = other.ProgressRatio;
        Type = other.Type;
        return DummyUpdateInfo.Instance;
    }
}

public enum ProgressItemType
{
    Default = 0,
    RecruitingFollower = 1,
    BuildTemple = 2,
    ConvertToFireTemple = 3,
    UnlockFireImp = 4,
    UnlockWarrior = 5,
}

public static class ProgressItemTypeHelpers
{
    public static ProgressItemType FromElement(Element element)
    {
        switch (element)
        {
            case Element.Fire: return ProgressItemType.ConvertToFireTemple;
            default:
                return ProgressItemType.Default;
        }
    }

    public static ProgressItemType FromTech(PlayerTech tech)
    {
        if (tech.FireTech.HasFlag(FireTech.FireImp))
        {
            return ProgressItemType.UnlockFireImp;
        }
        if (tech.NormalTech.HasFlag(NormalTech.Warrior))
        {
            return ProgressItemType.UnlockWarrior;
        }
        return ProgressItemType.Default;
    }
}