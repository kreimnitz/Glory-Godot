using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class Player
{
    public const int TempleCount = 3;

    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(4)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    [ProtoMember(5)]
    public virtual List<Enemy> Enemies { get; set; } = new();

    [ProtoMember(6)]
    public virtual List<TowerShot> TowerShots { get; set; } = new();

    [ProtoMember(7)]
    public List<Temple> Temples { get; set; } = new List<Temple>(TempleCount);

    [ProtoMember(8)]
    public PlayerTech Tech { get; set; } = new();

    public int FollowerCount => GetTotalFollowerCount();

    public Player()
    {
    }

    public PlayerUpdateInfo UpdateAndGetInfo(Player p)
    {
        Glory = p.Glory;
        UpdateUtilites.UpdateMany(Temples, p.Temples);
        UpdateUtilites.UpdateMany(TaskQueue, p.TaskQueue);

        PlayerUpdateInfo playerUpdateInfo = new();
        playerUpdateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, p.Enemies);
        playerUpdateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, p.TowerShots);
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
    public PlayerTech AddedTech { get; set; }
    public ListUpdateInfo<Enemy> EnemyUpdates { get; set; } = new();
    public ListUpdateInfo<TowerShot> TowerShotUpdates { get; set; } = new();
}