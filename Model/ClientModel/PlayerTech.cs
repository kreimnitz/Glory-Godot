using System;
using ProtoBuf;

[ProtoContract]
public class PlayerTech : IUpdateFrom<PlayerTech, PlayerTech>
{
    public static PlayerTech Empty { get; } = new();

    [ProtoMember(1)]
    public Guid Id { get; set; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public NormalTech NormalTech { get; set; }

    [ProtoMember(3)]
    public FireTech FireTech { get; set; }

    public PlayerTech() { }

    public PlayerTech(NormalTech normalTech, FireTech fireTech)
    {
        NormalTech = normalTech;
        FireTech = fireTech;
    }

    public PlayerTech UpdateFrom(PlayerTech other)
    {
        var newTech = new PlayerTech
        {
            NormalTech = other.NormalTech & ~NormalTech,
            FireTech = other.FireTech & ~FireTech
        };

        NormalTech |= other.NormalTech;
        FireTech |= other.FireTech;
        return newTech;
    }

    public bool IsEmpty()
    {
        if (NormalTech != NormalTech.None)
        {
            return false;
        }
        if (FireTech != FireTech.None)
        {
            return false;
        }
        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj as PlayerTech is null)
        {
            return false;
        }
        var other = (PlayerTech)obj;
        return FireTech == other.FireTech && NormalTech == other.NormalTech;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FireTech.GetHashCode(), NormalTech.GetHashCode());
    }

    public bool HasAllFlags(PlayerTech other)
    {
        var hasFireTech = (FireTech & other.FireTech) == other.FireTech;
        var hasNormalTech = (NormalTech & other.NormalTech) == other.NormalTech;
        return hasFireTech && hasNormalTech;
    }

    public static PlayerTech operator |(PlayerTech a, PlayerTech b)
    {
        return new PlayerTech
        {
            NormalTech = a.NormalTech | b.NormalTech,
            FireTech = a.FireTech | b.FireTech,
        };
    }

    public static PlayerTech operator |(PlayerTech a, NormalTech b)
    {
        return new PlayerTech
        {
            NormalTech = a.NormalTech | b,
            FireTech = a.FireTech,
        };
    }

    public static PlayerTech operator |(PlayerTech a, FireTech b)
    {
        return new PlayerTech
        {
            NormalTech = a.NormalTech,
            FireTech = a.FireTech | b,
        };
    }

    public static PlayerTech operator &(PlayerTech a, PlayerTech b)
    {
        return new PlayerTech
        {
            NormalTech = a.NormalTech & b.NormalTech,
            FireTech = a.FireTech & b.FireTech,
        };
    }
}

[Flags]
public enum NormalTech
{
    None = 0,
    Warrior = 1,
}

[Flags]
public enum FireTech
{
    None = 0,
    FireImp = 1,
}