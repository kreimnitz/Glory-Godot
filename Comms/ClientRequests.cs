using ProtoBuf;

public enum ClientRequestType
{
    PlayerRequest = 0,
    TempleRequest = 1,
    SummonRequest = 2,

    DEBUG_SpawnEnemy = 50,
    DEBUG_ResetPlayers = 51
}

public enum TempleRequest
{
    BuildTemple = 0,
    RecruitFollower = 1,
    ConvertToFireTemple = 10,
    UnlockFireImp = 20,
}

public enum PlayerRequest
{
    RecruitFollower = 0,
}

[ProtoContract]
public class PlayerRequestData
{
    [ProtoMember(1)]
    public PlayerRequest Request { get; set; }
}

[ProtoContract]
public class SummonRequestData
{
    [ProtoMember(1)]
    public UnitType Type { get; set; }
}

[ProtoContract]
public class TempleRequestData
{
    [ProtoMember(1)]
    public TempleRequest Request { get; set; }

    [ProtoMember(2)]
    public int TempleIndex { get; set; }
}