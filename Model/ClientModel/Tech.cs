using System.Collections.Generic;

public static class Tech
{
    public static TechInfo FireImpTechInfo = new()
    {
        Tech = PlayerTech.Empty | FireTech.FireImp,
        GloryCost = 150,
        ResearchDurationMs = 10000
    };

    public static TechInfo WarriorTechInfo = new()
    {
        Tech = PlayerTech.Empty | NormalTech.Warrior,
        GloryCost = 150,
        ResearchDurationMs = 10000
    };


    public static Dictionary<PlayerTech, TechInfo> TechToInfo { get; } = new()
    {
        { PlayerTech.Empty | FireTech.FireImp, FireImpTechInfo },
        { PlayerTech.Empty | NormalTech.Warrior, WarriorTechInfo },
    };
}

public class TechInfo
{
    public PlayerTech Tech { get; set; }
    public int GloryCost { get; set; }
    public int ResearchDurationMs { get; set; }
}