using System;
using System.Collections.Concurrent;

public class ActionQueue
{
    private ConcurrentQueue<Action> _actions = new();
    public void ExecuteActions()
    {
        while (_actions.TryDequeue(out Action tempAction)) tempAction();
    }

    public void Add(Action action)
    {
        _actions.Enqueue(action);
    }
}