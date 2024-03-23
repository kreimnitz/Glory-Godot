using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Timers;

public class Player
{
    public const int StartingGlory = 0;
    public const int StartingFollowerCount = 10;
    public const int IncomePerFollower = 1;
    public const int FollowerCost = 50;
    public const int FollowerTrainDuration = 10000;
    public const int IncomeTimerMs = 1000;

    private ActionQueue _actionQueue = new();
    private Timer _incomeTimer = new(IncomeTimerMs);

    public DelayedActionQueue InProgressQueue { get; } = new();

    public PlayerInfo Info { get; } = new();

    public Tower Tower { get; } = new();

    public List<Enemy> Enemies { get; } = new();

    public List<TowerShot> TowerShots { get; } = new();

    public Player()
    {
        Info.Glory = StartingGlory;
        Info.FollowerCount = StartingFollowerCount;
        _incomeTimer.Elapsed += (s, a) => _actionQueue.Add(() => ApplyIncome());
        _incomeTimer.Start();
    }

    public void DoLoop()
    {
        _actionQueue.ExecuteActions();
        InProgressQueue.ApplyNextActionIfReady();
        Info.TaskQueue = InProgressQueue.Actions.Select(da => da.ToProgressItem()).ToList();
        foreach (var enemy in Enemies)
        {
            enemy.DoLoop();
        }
        foreach (var towerShot in TowerShots)
        {
            towerShot.DoLoop();
        }

        CheckForNewShot();
        CheckLifetimes();
    }

    private void ApplyIncome()
    {
        Info.Glory += Info.FollowerCount * IncomePerFollower;
    }

    public void HandleAddFollowerRequest()
    {
        if (Info.Glory < FollowerCost)
        {
            return;
        }

        Info.Glory -= FollowerCost;
        var delayedAction = new DelayedAction(() => Info.FollowerCount++, FollowerTrainDuration);
        InProgressQueue.Enqueue(delayedAction);
    }

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void CheckForNewShot()
    {
        var shot = Tower.CheckForNewShot(Enemies);
        if (shot is not null)
        {
            TowerShots.Add(shot);
            Info.TowerShots.Add(shot.Info);
        }
    }

    public void CheckLifetimes()
    {
        int index = 0;
        while (index < Enemies.Count)
        {
            var enemy = Enemies[0];
            if (enemy.IsDead())
            {
                Enemies.RemoveAt(0);
                Info.Enemies.Remove(enemy.Info);
            }
            else
            {
                index++;
            }
        }

        index = 0;
        while (index < TowerShots.Count)
        {
            if (TowerShots[0].IsComplete)
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