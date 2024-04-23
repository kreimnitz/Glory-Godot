using System;
using ProtoBuf;

[ProtoContract]
public class PlayerTech
{
    [ProtoMember(1)]
    public BaseTech BaseTech { get; set; }

    [ProtoMember(2)]
    public FireTech FireTech { get; set; }

    public PlayerTech UpdateFrom(PlayerTech other)
    {
        var newTech = new PlayerTech();
        newTech.BaseTech = other.BaseTech & ~BaseTech;
        newTech.FireTech = other.FireTech & ~FireTech;

        BaseTech |= other.BaseTech;
        FireTech |= other.FireTech;
        return newTech;
    }
}

[Flags]
public enum BaseTech
{
    None = 0,
}

[Flags]
public enum FireTech
{
    None = 0,
    FlameImp = 1,
}