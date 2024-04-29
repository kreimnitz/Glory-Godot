using ProtoBuf;

public enum ClientRequestType
{
    PlayerRequest = 0,
    TempleRequest = 1,

    DEBUG_SpawnEnemy = 50,
    DEBUG_ResetPlayers = 51
}

public enum TempleRequest
{
    BuildTemple = 0,
    RecruitFollower = 1,

    ConvertToFireTemple = 10,
    UnlockFireImp = 20,
    SpawnFireImp = 21,
}

public enum PlayerRequest
{
    RecruitFollower = 0,
}

[ProtoContract]
public class TempleRequestData
{
    [ProtoMember(0)]
    public TempleRequest Request { get; set; }

    [ProtoMember(1)]
    public int TempleIndex { get; set; }
}