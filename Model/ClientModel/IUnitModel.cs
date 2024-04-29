public interface IUnitModel
{

    UnitType Type { get; }
    int HpCurrent { get; }
    int HpMax { get; }
}

public enum UnitType
{
    Default = 0,
    FireImp = 1,
}