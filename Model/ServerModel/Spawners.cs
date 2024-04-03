public static class Spawners
{
    public const int VentSpawnerTimerMs = 5000;
    public const int VentSpawnerCost = 150;
    public const int VentCreateDurationMs = 10000;
    public static ServerSpawner CreateVentSpawner(ServerTemple temple)
    {
        return new ServerSpawner(temple, VentSpawnerTimerMs, EnemyType.FireImp);
    }
}