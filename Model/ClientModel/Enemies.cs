using System.Collections.Generic;

public enum UnitType
{
    Default = 0,
    FireImp = 1,
    Warrior = 2,
}

public static class Enemies
{
    public static EnemyInfo FireImpInfo { get; } = new()
    {
        Name = "Fire Imp",
        Type = UnitType.FireImp,
        MaxHp = 10,
        Speed = 200,
        GloryCost = 50,
        Element = Element.Fire,
        SpawnTimerMs = 5000,
        SpawnMax = 10,
        RequiredTech = Tech.FireImpTechInfo,
    };

    public static EnemyInfo WarriorInfo { get; } = new()
    {
        Name = "Warrior",
        Type = UnitType.Warrior,
        MaxHp = 10,
        Speed = 200,
        GloryCost = 50,
        Element = Element.None,
        SpawnTimerMs = 5000,
        SpawnMax = 5,
        RequiredTech = Tech.WarriorTechInfo
    };

    public static Dictionary<UnitType, EnemyInfo> TypeToInfo { get; } = new()
    {
        { UnitType.Default, WarriorInfo },
        { UnitType.Warrior, WarriorInfo },
        { UnitType.FireImp, FireImpInfo },
    };

    public static Dictionary<TechInfo, EnemyInfo> TechToInfo { get; } = new()
    {
        { Tech.WarriorTechInfo, WarriorInfo },
        { Tech.FireImpTechInfo, FireImpInfo },
    };
}

public class EnemyInfo
{
    public string Name { get; set; }
    public UnitType Type { get; set; }
    public int MaxHp { get; set; }
    public int GloryCost { get; set; }
    public Element Element { get; set; }
    public int SpawnTimerMs { get; set; }
    public int SpawnMax { get; set; }
    public int Speed { get; set; }
    public TechInfo RequiredTech { get; set; }
}