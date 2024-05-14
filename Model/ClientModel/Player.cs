using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class Player : IUpdateFrom<Player, PlayerUpdateInfo>
{
    public const int StartingGlory = 1000;
    public const int StartingFollowerCount = 10;
    public const int HpMax = 20;

    [ProtoMember(1)]
    public Guid Id { get; private set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(3)]
    public Element Element { get; set; }

    [ProtoMember(4)]
    public int HpCurrent { get; set; } = HpMax;

    [ProtoMember(5)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    [ProtoMember(6)]
    public virtual List<Enemy> Enemies { get; set; } = new();

    [ProtoMember(7)]
    public virtual List<TowerShot> TowerShots { get; set; } = new();

    [ProtoMember(9)]
    public List<Temple> Temples { get; set; } = new List<Temple>();

    [ProtoMember(10)]
    public SummonGate SummonGate { get; set; } = new();

    [ProtoMember(11)]
    public PlayerTech Tech { get; set; } = new();

    public int TotalFollowerCount => GetTotalFollowerCount();

    public Player()
    {
    }

    public PlayerUpdateInfo UpdateFrom(Player p)
    {
        PlayerUpdateInfo playerUpdateInfo = new();
        if (Id != p.Id)
        {
            Id = p.Id;
            playerUpdateInfo.NewId = true;
        }
        Glory = p.Glory;
        HpCurrent = p.HpCurrent;
        if (Element != p.Element)
        {
            Element = p.Element;
            playerUpdateInfo.ElementAdded = true;
        }

        UpdateUtilites.UpdateMany<ProgressItem, DummyUpdateInfo>(TaskQueue, p.TaskQueue);
        playerUpdateInfo.SummonGateUpdates = SummonGate.UpdateFrom(p.SummonGate);  
        playerUpdateInfo.TempleUpdates = UpdateUtilites.UpdateMany<Temple, PropertyUpdateInfo>(Temples, p.Temples);
        playerUpdateInfo.EnemyUpdates = UpdateUtilites.UpdateMany<Enemy, PropertyUpdateInfo>(Enemies, p.Enemies);
        playerUpdateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany<TowerShot, PropertyUpdateInfo>(TowerShots, p.TowerShots);
        playerUpdateInfo.AddedTech = Tech.UpdateFrom(p.Tech);
        return playerUpdateInfo;
    }

    private int GetTotalFollowerCount()
    {
        int count = 0;
        for (int i = 0; i < Temples.Count; i++)
        {
            count += Temples[i]?.FollowerCount ?? 0;
        }
        return count;
    }
}

public class PlayerUpdateInfo
{
    public bool NewId { get; set; }
    public bool ElementAdded { get; set; }
    public PlayerTech AddedTech { get; set; }
    public SummonGateUpdateInfo SummonGateUpdates { get; set; }
    public ListUpdateInfo<Temple, PropertyUpdateInfo> TempleUpdates { get; set; }
    public ListUpdateInfo<Enemy, PropertyUpdateInfo> EnemyUpdates { get; set; }
    public ListUpdateInfo<TowerShot, PropertyUpdateInfo> TowerShotUpdates { get; set; }
}