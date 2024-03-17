using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class PlayerInfo
{
    [ProtoMember(1)]
    public int Glory { get; set; }

    [ProtoMember(2)]
    public int FollowerCount { get; set; }

    [ProtoMember(3)]
    public Queue<ProgressItem> TaskQueue { get; set; }

    public void UpdateFrom(PlayerInfo p)
    {
        Glory = p.Glory;
        FollowerCount = p.FollowerCount;
    }
}