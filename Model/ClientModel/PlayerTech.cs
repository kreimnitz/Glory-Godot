using System;
using ProtoBuf;

[ProtoContract]
public class PlayerTech
{
    [ProtoMember(1)]
    public BaseTech BaseTech { get; set; }

    [ProtoMember(2)]
    public FireTech FireTech { get; set; }

    public delegate void TechUpdateHandler(object sender, TechUpdateEventArgs e);
    public event TechUpdateHandler OnTechUpdate;
    private void RaiseTechUpdate(PlayerTech newTech)
    {
        // Make sure someone is listening to event
        if (OnTechUpdate == null) return;
        if (newTech.BaseTech == BaseTech.None && newTech.FireTech == FireTech.None)
        {
            return;
        }
        OnTechUpdate(this, new TechUpdateEventArgs() {NewTech = newTech});
    }

    public PlayerTech UpdateFrom(PlayerTech other)
    {
        var newTech = new PlayerTech
        {
            BaseTech = other.BaseTech & ~BaseTech,
            FireTech = other.FireTech & ~FireTech
        };

        BaseTech |= other.BaseTech;
        FireTech |= other.FireTech;
        RaiseTechUpdate(newTech);
        return newTech;
    }
}

public class TechUpdateEventArgs
{
    public PlayerTech NewTech { get; set; }
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