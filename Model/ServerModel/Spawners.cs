public static class Spawners
{
    public const int FireImpSpawnerTimerMs = 5000;
    public const int FireImpUnlockCost = 150;
    public const int FireImpUnlockDurationMs = 10000;
    public static ServerSpawner CreateFireImpSpawner()
    {
        return new ServerSpawner(FireImpSpawnerTimerMs, EnemyType.FireImp);
    }
}