using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class GameStateInfo
{
    [ProtoMember(1)]
    public virtual PlayerInfo Player1Info { get; set; }

    [ProtoMember(2)]
    public virtual PlayerInfo Player2Info { get; set; }
}