using System;
using System.Collections.Generic;

public class EventManager
{

    static EventManager _instance;

    public static EventManager instance
    {

        get
        {

            if (_instance == null)
                _instance = new EventManager();

            return _instance;

        }

    }

    public delegate void EventDelegate<T>(T e) where T : AppEvent;

    readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    public void AddListener<T>(EventDelegate<T> listener) where T : AppEvent
    {
        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            _delegates[typeof(T)] = Delegate.Combine(d, listener);
        }
        else
        {
            _delegates[typeof(T)] = listener;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> listener) where T : AppEvent
    {
        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            Delegate currentDel = Delegate.Remove(d, listener);

            if (currentDel == null)
            {
                _delegates.Remove(typeof(T));
            }
            else
            {
                _delegates[typeof(T)] = currentDel;
            }
        }
    }

    public void InvokeEvent<T>(T e) where T : AppEvent
    {
        if (e == null)
        {
            throw new ArgumentNullException("e");
        }

        Delegate d;
        if (_delegates.TryGetValue(typeof(T), out d))
        {
            EventDelegate<T> callback = d as EventDelegate<T>;
            if (callback != null)
            {
                callback(e);
            }
        }
    }

    public void resetEventManager()
    {
        _delegates.Clear();
    }

}

