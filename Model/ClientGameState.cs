using System;
using System.Collections.Generic;

public class ClientGameState
{
    public PlayerInfo Player { get; set; } = new();

    public Dictionary<Guid, EnemyInfo> Enemies { get; } = new();

    public Dictionary<Guid, TowerShotInfo> TowerShots { get; } = new();

    public GameStateUpdateInfo UpdateFrom(GameStateInfo serverGameState)
    {
        var updateInfo = new GameStateUpdateInfo();
        Player.UpdateFrom(serverGameState.Player);

        updateInfo.EnemyUpdates = UpdateUtilites.UpdateMany(Enemies, serverGameState.Enemies);
        updateInfo.TowerShotUpdates = UpdateUtilites.UpdateMany(TowerShots, serverGameState.TowerShots);

        return updateInfo;
    }
}

public class GameStateUpdateInfo
{
    public UpdateInfo<EnemyInfo> EnemyUpdates { get; set; } = new();
    public UpdateInfo<TowerShotInfo> TowerShotUpdates { get; set; } = new();
}