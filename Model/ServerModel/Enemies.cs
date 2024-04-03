public static class EnemyUtilites
{
    private const int FireImpHp = 10;
    public const int FireImpCost = 50;
    public static ServerEnemy CreateFireImp()
    {
        var enemy = new ServerEnemy(FireImpHp);
        return enemy;
    }
}

public enum EnemyType
{
    FireImp = 1,
}