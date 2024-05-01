using System.Collections.Generic;
using Godot;

public static class ResourceHelpers
{
    public static readonly Dictionary<ProgressItemType, Texture2D> ProgressItemTextures = new()
    {
        { ProgressItemType.RecruitingFollower, Resources.FollowerIcon },
        { ProgressItemType.ConvertToFireTemple, Resources.FlameIcon },
        { ProgressItemType.UnlockFireImp, Resources.ImpIcon },
        { ProgressItemType.BuildTemple, Resources.TempleIcon }
    };

    public static readonly Dictionary<UnitType, Texture2D> UnitTypeToIcon = new()
    {
        { UnitType.Default, Resources.BlankIcon },
        { UnitType.FireImp, Resources.ImpIcon },
        { UnitType.Warrior, Resources.WarriorIcon },
    };

    public static readonly Dictionary<UnitType, Texture2D> UnitTypeToTexture = new()
    {
        { UnitType.Default, Resources.BlankIcon },
        { UnitType.FireImp, Resources.Imp },
        { UnitType.Warrior, Resources.Warrior },
    };
}