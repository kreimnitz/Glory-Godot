using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerPlayer))]
public class Player : IUpdateFrom<Player>
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(3)]
    public int FollowerCount { get; set; }

    [ProtoMember(4)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    [ProtoMember(5)]
    public virtual List<Enemy> Enemies { get; set; } = new();

    [ProtoMember(6)]
    public virtual List<TowerShot> TowerShots { get; set; } = new();

    [ProtoMember(7)]
    public Temple FireTemple { get; set; }

    public void UpdateFrom(Player p)
    {
        UpdateAndGetInfo(p);
    }

    public PlayerUpdateInfo UpdateAndGetInfo(Player p)
    {
        Glory = p.Glory;
        FollowerCount = p.FollowerCount;
        FireTemple.UpdateFrom(p.FireTemple);
        UpdateUtilites.UpdateMany(TaskQueue, p.TaskQueue);

        PlayerUpdateInfo playerUpdateInfo = new();
        playerUpdateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, p.Enemies);
        playerUpdateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, p.TowerShots);
        return playerUpdateInfo;
    }
}

public class PlayerUpdateInfo
{
    public UpdateInfo<Enemy> EnemyUpdates { get; set; } = new();
    public UpdateInfo<TowerShot> TowerShotUpdates { get; set; } = new();
}