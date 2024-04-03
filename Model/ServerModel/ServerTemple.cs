using System.Linq;

public class ServerTemple : Temple
{
    public const int CreateDurationMs = 20000;
    public const int Cost = 150;

    public const string FireTempleName = "FireRegion";
    public ServerTemple(string name)
    {
        Name = name;
    }

    public ServerSpawner GetSpawnerForType(EnemyType enemyType)
    {
        if (!IsActive)
        {
            return null;
        }
        return (ServerSpawner)Spawners.FirstOrDefault(s => s.EnemyType == enemyType);
    }
}