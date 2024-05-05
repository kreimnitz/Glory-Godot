using System;
using ProtoBuf;

[ProtoContract]
public class Spawner : IUpdateFrom<Spawner, PropertyUpdateInfo>, IProgressInfo
{
    [ProtoMember(1)]
    public Guid Id { get; } = IdGenerator.Generate();

    [ProtoMember(2)]
    public int CurrentValue { get; set; }

    [ProtoMember(3)]
    public int Max { get; set; }

    [ProtoMember(4)]
    public UnitType UnitType { get; set; }

    public PropertyUpdateInfo UpdateFrom(Spawner other)
    {
        PropertyUpdateInfo updateInfo = new();
        if (CurrentValue != other.CurrentValue)
        {
            CurrentValue = other.CurrentValue;
            updateInfo.Add(nameof(CurrentValue));
        }
        if (Max != other.Max)
        {
            Max = other.Max;
            updateInfo.Add(nameof(Max));
        }
        if (UnitType != other.UnitType)
        {
            UnitType = other.UnitType;
            updateInfo.Add(nameof(UnitType));
        }
        return updateInfo;
    }
}