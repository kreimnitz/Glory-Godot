using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class GameStateInfo
{
    [ProtoMember(1)]
    public virtual PlayerInfo Player { get; } = new PlayerInfo();

    [ProtoMember(2)]
    public virtual List<EnemyInfo> Enemies { get; } = new List<EnemyInfo>();

    [ProtoMember(3)]
    public virtual List<TowerShotInfo> TowerShots { get; } = new List<TowerShotInfo>();
}