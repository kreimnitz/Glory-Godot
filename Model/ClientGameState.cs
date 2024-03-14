using System;
using System.Collections.Generic;
using System.Linq;

public class ClientGameState
{
    public Player Player { get; set; } = new Player();

    public Dictionary<Guid, Enemy> Enemies { get; } = new();

    public Dictionary<Guid, TowerShot> TowerShots { get; } = new();

    public GameStateUpdateInfo UpdateFrom(ConcurrentGameState serverGameState)
    {
        var updateInfo = new GameStateUpdateInfo();
        Player.UpdateFrom(serverGameState.Player);
        updateInfo.NewEnemies = UpdateEnemiesFromGameState(serverGameState);

        return updateInfo;
    }

    private List<Enemy> UpdateEnemiesFromGameState(ConcurrentGameState serverGameState)
    {
        var newEnemies = new List<Enemy>();
        foreach (var serverEnemy in serverGameState.Enemies)
        {
            if (Enemies.TryGetValue(serverEnemy.Id, out Enemy matchingEnemy))
            {
                matchingEnemy.UpdateFrom(serverEnemy);
            }
            else
            {
                Enemies.Add(serverEnemy.Id, serverEnemy);
                newEnemies.Add(serverEnemy);
            }
        }

        var serverIds = serverGameState.Enemies.Select(e => e.Id).ToHashSet();
        foreach (var enemy in Enemies.Values.ToList())
        {
            if (serverIds.Contains(enemy.Id))
            {
                continue;
            }
            enemy.IsZombie = true;
            Enemies.Remove(enemy.Id);
        }

        return newEnemies;
    }

    private List<TowerShot> UpdateTowerShotsFromGameState(ConcurrentGameState serverGameState)
    {
        var newTowerShots = new List<TowerShot>();
        foreach (var serverTowerShot in serverGameState.TowerShots)
        {
            if (TowerShots.TryGetValue(serverTowerShot.Id, out TowerShot matchingEnemy))
            {
                matchingEnemy.UpdateFrom(serverEnemy);
            }
            else
            {
                Enemies.Add(serverEnemy.Id, serverEnemy);
                newEnemies.Add(serverEnemy);
            }
        }
    }
}

public class GameStateUpdateInfo
{
    public List<Enemy> NewEnemies { get; set; } = new();
    public List<TowerShot> NewTowerShots { get; set; } = new();
}