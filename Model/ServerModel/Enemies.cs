using System.IO;
using Godot;

public static class EnemyUtilites
{
    private const int FireImpHp = 10;
    public const int FireImpCost = 50;
    public static ServerEnemy CreateFireImp(EnemyPath path)
    {
        var enemy = new ServerEnemy(FireImpHp, path);
        return enemy;
    }
}

public enum EnemyType
{
    FireImp = 1,
}