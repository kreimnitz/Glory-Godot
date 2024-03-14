using System;
using ProtoBuf;

[ProtoContract]
public class Player
{
    private const int IncomePerFollower = 50;

    [ProtoMember(1)]
    public int Glory { get; set; } = 0;

    [ProtoMember(2)]
    public int FollowerCount { get; set; } = 1;

    public void UpdateFrom(Player p)
    {
        Glory = p.Glory;
        FollowerCount = p.FollowerCount;
    }

    public void ApplyIncome()
    {
        Glory += FollowerCount * IncomePerFollower;
    }
}