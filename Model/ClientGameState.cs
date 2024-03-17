using System;
using System.Collections.Generic;
using System.Linq;

public class ClientGameState
{
    public PlayerInfo Player { get; set; } = new();

    public Dictionary<Guid, EnemyInfo> Enemies { get; } = new();

    public Dictionary<Guid, TowerShotInfo> TowerShots { get; } = new();

    public GameStateUpdateInfo UpdateFrom(GameStateInfo serverGameState)
    {
        var updateInfo = new GameStateUpdateInfo();
        Player.UpdateFrom(serverGameState.Player);
        (updateInfo.NewEnemies, updateInfo.RemovedEnemies) = UpdateEnemies(serverGameState);
        (updateInfo.NewTowerShots, updateInfo.RemovedTowerShots) = UpdateTowerShots(serverGameState);

        return updateInfo;
    }

    private (List<EnemyInfo> NewEnemies, List<EnemyInfo> RemovedEnemies) UpdateEnemies(GameStateInfo serverGameState)
    {
        var newEnemies = new List<EnemyInfo>();
        var removedEnemies = new List<EnemyInfo>();
        foreach (var serverEnemy in serverGameState.Enemies)
        {
            if (Enemies.TryGetValue(serverEnemy.Id, out EnemyInfo matchingEnemy))
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

    private (List<TowerShotInfo> NewShots, List<TowerShotInfo> RemovedShots) UpdateTowerShots(GameStateInfo serverGameState)
    {
        var newTowerShots = new List<TowerShotInfo>();
        var removedTowerShots = new List<TowerShotInfo>();
        foreach (var serverTowerShot in serverGameState.TowerShots)
        {
            if (TowerShots.TryGetValue(serverTowerShot.Id, out TowerShotInfo matchingTowerShot))
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
    public List<EnemyInfo> NewEnemies { get; set; } = new();
    public List<EnemyInfo> RemovedEnemies { get; set; } = new();
    public List<TowerShotInfo> NewTowerShots { get; set; } = new();
    public List<TowerShotInfo> RemovedTowerShots { get; set; } = new();
}