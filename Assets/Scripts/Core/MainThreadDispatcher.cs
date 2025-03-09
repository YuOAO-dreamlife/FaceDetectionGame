using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> executionQueue = new Queue<Action>();

    public static void Enqueue(Action action)
    {
        if (action == null) // 檢查傳入的動作是否為 null
        {
            throw new ArgumentNullException(nameof(action));
        }

        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }

    void Update()
    {
        // 持續從Queue中取出動作，然後依序執行它們
        // 當Queue空了，就跳出迴圈
        while (true)
        {
            Action action = null;
            lock (executionQueue)
            {
                if (executionQueue.Count > 0)
                {
                    action = executionQueue.Dequeue();
                }
            }
            if (action == null)
            {
                break;
            }
            action();
        }
    }
}
