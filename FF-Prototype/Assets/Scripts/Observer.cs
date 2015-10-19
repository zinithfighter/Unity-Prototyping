using UnityEngine;
using System.Collections;
using System;

public class Observer : MonoBehaviour, IPublisher, ISubscriber
{

    #region Interfaces
    public void Publish(MessageLayer m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Publish<T>(MessageLayer m, string e, T args)
    {
        EventSystem.Broadcast(m, e, args);
    }

    public void Publish<T,V>(MessageLayer m, string e, T arg1, V arg2)
    {
        EventSystem.Broadcast(m, e, arg1, arg2);
    }

    public void Subscribe(MessageLayer t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }

    public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }
    public void Subscribe<T,V>(MessageLayer t, string e, Callback<T,V> c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }
    #endregion Interfaces


}
