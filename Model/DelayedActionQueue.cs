using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class DelayedActionQueue
{
    private ConcurrentQueue<DelayedAction> _queue = new();

    public IEnumerable<DelayedAction> Actions => _queue;

    public void Enqueue(DelayedAction action)
    {
        _queue.Enqueue(action);
        if (_queue.Count == 1)
        {
            action.Start();
        }
    }

    public void ApplyNextActionIfReady()
    {
        if (_queue.TryPeek(out DelayedAction delayedAction) && delayedAction.Ready)
        {
            if (_queue.TryDequeue(out delayedAction))
            {
                delayedAction.Action();
                if (_queue.TryPeek(out DelayedAction nextAction))
                {
                    nextAction.Start();
                }
            }
        }
    }
}