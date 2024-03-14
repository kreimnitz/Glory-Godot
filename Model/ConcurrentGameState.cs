using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ProtoBuf;

[ProtoContract]
public class ConcurrentGameState
{
    private object _lock = new();

    private Tower Tower { get; set; } =  new Tower();

    [ProtoMember(1)]
    public Player Player { get; set; } = new Player();

    [ProtoMember(2)]
    public List<Enemy> Enemies { get; } =  new List<Enemy>();

    [ProtoMember(3)]
    public List<TowerShot> TowerShots { get; } = new List<TowerShot>();

    public void AddEnemy(Enemy enemy)
    {
        lock (_lock)
        {
            Enemies.Add(enemy);
        }
    }

    public void UpdateProgress()
    {
        lock (_lock)
        {
            foreach (var enemy in Enemies)
            {
                enemy.UpdateProgressRatio();
            }
            foreach (var towerShot in TowerShots)
            {
                towerShot.UpdateProgressRatio();
            }
        }
    }

    public void CheckForNewShot()
    {
        lock (_lock)
        {
            var shot = Tower.CheckForNewShot(Enemies);
            if (shot is not null)
            {
                TowerShots.Add(shot);
            }
        }
    }

    public void ApplyIncome()
    {
        lock (_lock)
        {
            Player.ApplyIncome();
        }
    }

    public void CheckLifetimes()
    {
        lock (_lock)
        {
            int index = 0;
            while (index < Enemies.Count)
            {
                if (Enemies[0].ProgressRatio >= 1)
                {
                    Enemies.RemoveAt(0);
                }
                else
                {
                    index++;
                }
            }

            index = 0;
            while (index < TowerShots.Count)
            {
                if (TowerShots[0].ProgressRatio >= 1)
                {
                    TowerShots.RemoveAt(0);
                }
                else
                {
                    index++;
                }
            }
        }
    }
}