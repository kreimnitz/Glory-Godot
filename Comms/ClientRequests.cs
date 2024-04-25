using ProtoBuf;

public enum ClientRequestType
{
    AddFollower = 1,

    UpgradeTowerDamage = 2,
    UpgradeTowerAttackSpeed = 3,

    ConvertToFireTemple = 10,

    UnlockFireImp = 20,
    SpawnFireImp = 21,

    DEBUG_SpawnEnemy = 50,
    DEBUG_ResetPlayers = 51
}

[ProtoContract]
public class TempleIndexData
{
    [ProtoMember(1)]
    public int TempleIndex { get; set; }
}