using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

[ProtoContract]
public class Player
{
    public const int StartingGlory = 1000;
    public const int StartingFollowerCount = 10;
    public const int TempleCount = 3;
    public const int HpMax = 20;

    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(3)]
    public int FollowerCount { get; set; }

    [ProtoMember(4)]
    public int HpCurrent { get; set; } = HpMax;

    [ProtoMember(5)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    [ProtoMember(6)]
    public virtual List<Enemy> Enemies { get; set; } = new();

    [ProtoMember(7)]
    public virtual List<TowerShot> TowerShots { get; set; } = new();

    [ProtoMember(8)]
    public List<Temple> Temples { get; set; } = new List<Temple>();

    [ProtoMember(9)]
    public SummonGate SummonGate { get; set; } = new();

    [ProtoMember(10)]
    public PlayerTech Tech { get; set; } = new();

    public int TotalFollowerCount => GetTotalFollowerCount();

    public Player()
    {
    }

    public static Player Create()
    {
        var p = new Player();
        for (int i = 0; i < TempleCount; i++)
        {
            p.Temples.Add(new());
        }
        return p;
    }

    public PlayerUpdateInfo UpdateAndGetInfo(Player p)
    {
        Glory = p.Glory;
        HpCurrent = p.HpCurrent;
        FollowerCount = p.FollowerCount;
        SummonGate.UpdateFrom(p.SummonGate);
        for (int i = 0; i < TempleCount; i++)
        {
            Temples[i].UpdateFrom(p.Temples[i]);
        }
        UpdateUtilites.UpdateMany(TaskQueue, p.TaskQueue);

        PlayerUpdateInfo playerUpdateInfo = new();
        playerUpdateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, p.Enemies);
        playerUpdateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, p.TowerShots);
        playerUpdateInfo.AddedTech = Tech.UpdateFrom(p.Tech);
        return playerUpdateInfo;
    }

    private int GetTotalFollowerCount()
    {
        int count = FollowerCount;
        for (int i = 0; i < Temples.Count; i++)
        {
            count += Temples[i]?.FollowerCount ?? 0;
        }
        return count;
    }
}

public class PlayerUpdateInfo
{
    public PlayerTech AddedTech { get; set; }
    public ListUpdateInfo<Enemy> EnemyUpdates { get; set; } = new();
    public ListUpdateInfo<TowerShot> TowerShotUpdates { get; set; } = new();
}