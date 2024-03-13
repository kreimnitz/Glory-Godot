using System.Collections.Generic;

public class GameState
{
    Player Player { get; set; }
    List<Enemy> Enemies { get; } =  new List<Enemy>();
    List<TowerShot> TowerShots { get; } = new List<TowerShot>();
}