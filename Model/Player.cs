using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class Player
{
    public const int IncomePerFollower = 50;
    public const int FollowerCost = 500;
    public const int FollowerDelayMs = 3000;

    public Queue<DelayedAction> AddFollowersQueue { get; } = new();

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