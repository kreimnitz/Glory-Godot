using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class GameStateInfo
{
    [ProtoMember(1)]
    public virtual PlayerInfo Player { get; set; }

    [ProtoMember(2)]
    public virtual List<EnemyInfo> Enemies { get; set; } = new();

    [ProtoMember(3)]
    public virtual List<TowerShotInfo> TowerShots { get; set; } = new();
}