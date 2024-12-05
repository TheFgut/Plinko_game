using System;
using System.Collections.Concurrent;
using UnityEngine;

public class UnityMainThreadActionLauncher : MonoBehaviour
{
    private static readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

    public void Enqueue(Action action)
    {
        _actions.Enqueue(action);
    }

    private void Update()
    {
        while (_actions.TryDequeue(out var action))
        {
            action();
        }
    }
}
