using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
[ProtoInclude(500, typeof(ServerPlayer))]
public class Player : IUpdateFrom<Player>
{
    public const int TempleCount = 3;

    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(4)]
    public virtual List<ProgressItem> TaskQueue { get; protected set; } = new();

    [ProtoMember(5)]
    public virtual List<Enemy> Enemies { get; set; } = new();

    [ProtoMember(6)]
    public virtual List<TowerShot> TowerShots { get; set; } = new();

    [ProtoMember(7)]
    public Temple[] Temples { get; set; } = new Temple[3];

    [ProtoMember(8)]
    public PlayerTech Tech { get; set; } = new();

    public void UpdateFrom(Player p)
    {
        UpdateAndGetInfo(p);
    }

    public PlayerUpdateInfo UpdateAndGetInfo(Player p)
    {
        Glory = p.Glory;
        UpdateTemples(p.Temples);
        UpdateUtilites.UpdateMany(TaskQueue, p.TaskQueue);

        PlayerUpdateInfo playerUpdateInfo = new();
        playerUpdateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, p.Enemies);
        playerUpdateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, p.TowerShots);
        return playerUpdateInfo;
    }

    private void UpdateTemples(Temple[] otherTemples)
    {
        for (int i = 0; i < TempleCount; i++)
        {
            if (Temples[i] == null)
            {
                continue;
            }
            Temples[i].UpdateFrom(otherTemples[i]);
        }
    }
}

public class PlayerUpdateInfo
{
    public UpdateInfo<Enemy> EnemyUpdates { get; set; } = new();
    public UpdateInfo<TowerShot> TowerShotUpdates { get; set; } = new();
}