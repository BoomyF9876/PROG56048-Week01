using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();
    private static readonly object padlock = new object();

    public static void Subscribe<T>(Action<T> action)
    {
        lock (padlock)
        {
            Type t = typeof(T);
            //if (!subscribers.ContainsKey(t))
        }
    }
    public static void Unsubscribe<T>(Action<T> action) { }
    public static void Publish<T>(T eventData) { }
}
