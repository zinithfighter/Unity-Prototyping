using UnityEngine;
using System;
using System.Collections.Generic;


static public class EventSystem 
{
    static private Dictionary<string, Delegate> _eventTable = new Dictionary<string, Delegate>();

    static private List<string> _subscribers = new List<string>();

    /// <summary>
    /// Notify all the subscribers that a message has occurred
    /// </summary>
    /// <param name="message"></param>
    static public void Broadcast(MessageLayer t, string e)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();
        //Debug.Log("Event Broadcast: " + message);
        Delegate d;
        if (_eventTable.TryGetValue(eventType, out d))
        {
            //Debug.Log("execute " + message);
            Callback s = d as Callback;
            if (s != null)
                s();
        }
    }

    static public void Broadcast<T>(MessageLayer t, string e, T arg)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();
       // Debug.Log("Event Broadcast: " + eventType);
        Delegate d;
        if (_eventTable.TryGetValue(eventType, out d))
        {
            //Debug.Log("execute " + message);
            Callback<T> s = d as Callback<T>;
            if (s != null)
                s(arg);
        }
    }

    static public void Broadcast<T,V>(MessageLayer t, string e, T arg1, V arg2)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();
        // Debug.Log("Event Broadcast: " + eventType);
        Delegate d;
        if (_eventTable.TryGetValue(eventType, out d))
        {
            //Debug.Log("execute " + message);
            Callback<T,V> s = d as Callback<T,V>;
            if (s != null)
                s(arg1, arg2);
        }
    }


    /// <summary>
    /// subscribe to a message without providing arguments for the delegate
    /// </summary>
    /// <param name="t">the type of message</param>
    /// <param name="e">the message to listen for</param>
    /// <param name="sub">the object that implements the interface</param>
    static public bool Subscribe(MessageLayer t, string e, Callback c, ISubscriber s)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();

        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = (Callback)_eventTable[eventType] + c;
            _subscribers.Add(s.ToString() + ":" + eventType);
            //if it has the key we append it

            return true;
        }

        _eventTable.Add(eventType, c);

        return true;
    }

    /// <summary>
    /// subscribe to the message
    /// </summary>
    /// <typeparam name="T">argument type of the delegate</typeparam>
    /// <param name="t">type of message</param>
    /// <param name="e">message</param>
    /// <param name="c">the event listener</param>
    /// <param name="s">subscriber of the message</param>
    /// <returns></returns>
    static public bool Subscribe<T>(MessageLayer t, string e, Callback<T> c, ISubscriber s)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();

        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = (Callback<T>)_eventTable[eventType] + c;
            _subscribers.Add(s.ToString() + ":" + eventType);
            return true;
        }

        _subscribers.Add(s.ToString() + ":" + eventType);
        _eventTable.Add(eventType, c);

        return true;
    }

    /// <summary>
    /// subscribe to the message
    /// </summary>
    /// <typeparam name="T">argument type of the delegate</typeparam>
    /// <param name="t">type of message</param>
    /// <param name="e">message</param>
    /// <param name="c">the event listener</param>
    /// <param name="s">subscriber of the message</param>
    /// <returns></returns>
    static public bool Subscribe<T,V>(MessageLayer t, string e, Callback<T,V> c, ISubscriber s)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();

        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = (Callback<T,V>)_eventTable[eventType] + c;
            _subscribers.Add(s.ToString() + ":" + eventType);
            return true;
        }

        _subscribers.Add(s.ToString() + ":" + eventType);
        _eventTable.Add(eventType, c);

        return true;
    }

    static public void RemoveSubscriber(MessageLayer t, string e, ISubscriber s)
    {
        string eventType = format(t, e).Replace(" ", string.Empty).ToLower();
        _subscribers.Remove(s.ToString() + ":" + eventType);
    }

    /// <summary>
    /// format a message with its type and message
    /// </summary>
    /// <param name="t">type of message</param>
    /// <param name="m">message listening for</param>
    /// <returns></returns>
    static public string format(MessageLayer t, string m)
    {
        return t.ToString() + ":" + m;
    }

    static public List<string> Subscribers
    {
        get { return _subscribers; }
    }


}

