using System.Collections.Generic;

public enum UnitType
{
    Default = 0,
    FireImp = 1,
    Warrior = 2,
}

public static class Enemies
{
    public static EnemyInfo FireImpInfo = new()
    {
        Name = "Fire Imp",
        Type = UnitType.FireImp,
        MaxHp = 10,
        Speed = 200,
        GloryCost = 50,
        Element = Element.Fire,
        UnlockGloryCost = 150,
        UnlockDuration = 10000,
        SpawnTimerMs = 5000,
        SpawnMax = 10,
        RequiredTech = new PlayerTech() | FireTech.FlameImp,
    };

    public static EnemyInfo WarriorInfo = new()
    {
        Name = "Warrior",
        Type = UnitType.Warrior,
        MaxHp = 10,
        Speed = 200,
        GloryCost = 50,
        Element = Element.None,
        UnlockGloryCost = 150,
        UnlockDuration = 5000,
        SpawnTimerMs = 5000,
        SpawnMax = 5,
        RequiredTech = new PlayerTech() | BaseTech.Warrior,
    };

    public static Dictionary<UnitType, EnemyInfo> TypeToInfo { get; } = new()
    {
        { UnitType.Default, WarriorInfo },
        { UnitType.Warrior, WarriorInfo },
        { UnitType.FireImp, FireImpInfo },
    };
}

public class EnemyInfo
{
    public string Name { get; set; }
    public UnitType Type { get; set; }
    public int MaxHp { get; set; }
    public int GloryCost { get; set; }
    public Element Element { get; set; }
    public int UnlockGloryCost { get; set; }
    public int UnlockDuration { get; set; }
    public int SpawnTimerMs { get; set; }
    public int SpawnMax { get; set; }
    public int Speed { get; set; }
    public PlayerTech RequiredTech { get; set; }
}