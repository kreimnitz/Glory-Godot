using System;
using System.Collections.Generic;

public class ClientGameState
{
    public Player Player { get; set; } = new();

    public Dictionary<Guid, Enemy> Enemies { get; } = new();

    public Dictionary<Guid, TowerShot> TowerShots { get; } = new();

    public GameStateUpdateInfo UpdateFrom(GameStateInfo serverGameState)
    {
        var updateInfo = new GameStateUpdateInfo();
        Player.UpdateFrom(serverGameState.PlayerInfo);

        updateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, serverGameState.PlayerInfo.Enemies);
        updateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, serverGameState.PlayerInfo.TowerShots);

        return updateInfo;
    }
}

public class GameStateUpdateInfo
{
    public UpdateInfo<Enemy> EnemyUpdates { get; set; } = new();
    public UpdateInfo<TowerShot> TowerShotUpdates { get; set; } = new();
}