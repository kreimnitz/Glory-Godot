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
        (updateInfo.NewEnemies, updateInfo.RemovedEnemies) = UpdateEnemies(serverGameState);
        (updateInfo.NewTowerShots, updateInfo.RemovedTowerShots) = UpdateTowerShots(serverGameState);

        return updateInfo;
    }

    private (List<Enemy> NewEnemies, List<Enemy> RemovedEnemies) UpdateEnemies(ConcurrentGameState serverGameState)
    {
        var newEnemies = new List<Enemy>();
        var removedEnemies = new List<Enemy>();
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
            Enemies.Remove(enemy.Id);
            removedEnemies.Add(enemy);
        }

        return (newEnemies, removedEnemies);
    }

    private (List<TowerShot> NewShots, List<TowerShot> RemovedShots) UpdateTowerShots(ConcurrentGameState serverGameState)
    {
        var newTowerShots = new List<TowerShot>();
        var removedTowerShots = new List<TowerShot>();
        foreach (var serverTowerShot in serverGameState.TowerShots)
        {
            if (TowerShots.TryGetValue(serverTowerShot.Id, out TowerShot matchingTowerShot))
            {
                matchingTowerShot.UpdateFrom(serverTowerShot);
            }
            else
            {
                TowerShots.Add(serverTowerShot.Id, serverTowerShot);
                newTowerShots.Add(serverTowerShot);
            }
        }

        var serverIds = serverGameState.TowerShots.Select(e => e.Id).ToHashSet();
        foreach (var towerShot in TowerShots.Values.ToList())
        {
            if (serverIds.Contains(towerShot.Id))
            {
                continue;
            }
            TowerShots.Remove(towerShot.Id);
            removedTowerShots.Add(towerShot);
        }

        return (newTowerShots, removedTowerShots);
    }
}

public class GameStateUpdateInfo
{
    public List<Enemy> NewEnemies { get; set; } = new();
    public List<Enemy> RemovedEnemies { get; set; } = new();
    public List<TowerShot> NewTowerShots { get; set; } = new();
    public List<TowerShot> RemovedTowerShots { get; set; } = new();
}