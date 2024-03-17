using System;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class PlayerInfo : IUpdateFrom<PlayerInfo>
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int Glory { get; set; }

    [ProtoMember(3)]
    public int FollowerCount { get; set; }

    [ProtoMember(4)]
    public virtual List<ProgressItem> TaskQueue { get; set; } = new();

    public void UpdateFrom(PlayerInfo p)
    {
        Glory = p.Glory;
        FollowerCount = p.FollowerCount;
        UpdateUtilites.UpdateMany(TaskQueue, p.TaskQueue);
    }
}