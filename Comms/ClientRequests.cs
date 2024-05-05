using ProtoBuf;

public enum ClientRequestType
{
    BuildTemple = 0,
    TempleActionRequest = 1,
    TempleTechRequest = 2,
    SummonRequest = 3,
    EnemyTechRequest = 4,

    DEBUG_SpawnEnemy = 50,
    DEBUG_ResetPlayers = 51
}

public enum TempleActionRequest
{
    RecruitFollower = 1,
    ConvertToFireTemple = 10,
}

public enum TempleTechRequest
{
}

[ProtoContract]
public class BuildTempleRequestData
{
    [ProtoMember(1)]
    public int Position { get; set; }
}

[ProtoContract]
public class SummonRequestData
{
    [ProtoMember(1)]
    public UnitType Type { get; set; }
}

public interface TempleIndexData
{
    int TempleIndex { get; set; }
}

[ProtoContract]
public class TempleActionRequestData : TempleIndexData
{
    [ProtoMember(1)]
    public TempleActionRequest Request { get; set; }

    [ProtoMember(2)]
    public int TempleIndex { get; set; }
}

[ProtoContract]
public class TempleTechRequestData : TempleIndexData
{
    [ProtoMember(1)]
    public PlayerTech Tech { get; set; }

    [ProtoMember(2)]
    public int TempleIndex { get; set; }
}

[ProtoContract]
public class EnemyTechRequestData
{
    [ProtoMember(1)]
    public UnitType EnemyType { get; set; }
}