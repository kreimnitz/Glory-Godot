using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class GameStateInfo
{
    [ProtoMember(1)]
    public virtual Player PlayerInfo { get; set; }
}