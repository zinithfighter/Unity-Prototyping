using UnityEngine;
using System;
using System.Collections.Generic;


static public class EventSystem
{
    static private List<object> _subscribers = new List<object>();

    static private Dictionary<string, Delegate> _eventTable = new Dictionary<string, Delegate>();

    /// <summary>
    /// Notify all the subscribers that a message has occurred
    /// </summary>
    /// <param name="message"></param>
    static public void Broadcast(MessageType m, string e)
    {
        string message = format(m, e);
        Debug.Log("Event Broadcast: " + message);
        Delegate d;
        if (_eventTable.TryGetValue(message, out d))
        {
            //Debug.Log("execute " + message);
            Callback s = d as Callback;
            if (s != null)
                s();
        }
    }

    static public void Broadcast<T>(MessageType m, string e, T arg)
    {
        string message = format(m, e);
        Debug.Log("Event Broadcast: " + message);
        Delegate d;
        if (_eventTable.TryGetValue(message, out d))
        {
            //Debug.Log("execute " + message);
            Callback<T> s = (Callback < T >)d;
            if (s != null)
                s(arg);
        }
    }

    static private void RemoveSubscriber(string m, Callback handler, ISubscriber go)
    {
        if (_eventTable.ContainsKey(m))
        {
            _eventTable[m] = (Callback)_eventTable[m] - handler;
        }
        //List<string> messagesToRemove = new List<string>();

        //foreach (KeyValuePair<string, Delegate> pair in _eventTable)
        //{
        //    bool wasFound = false;        

        //    if (!wasFound)
        //        messagesToRemove.Add(pair.Key);
        //}

        //foreach (string message in messagesToRemove)
        //{
        //    _eventTable.Remove(message);
        //}

    }

    /// <summary>
    /// subscribe to a message
    /// </summary>
    /// <param name="t">the type of message</param>
    /// <param name="e">the message to listen for</param>
    /// <param name="sub">the object that implements the interface</param>
    static public bool Subscribe(MessageType t, string e, Callback c, ISubscriber s)
    {
        string eventType = format(t, e);
        Subscriber sub = new Subscriber(t, e, c, s);       

        object obj = sub as object;
        _subscribers.Add(obj);
        
        if(_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = (Callback)_eventTable[eventType] + c;
            
            return true;
        }

        _eventTable.Add(eventType, c);

        return true;
    }

    /// <summary>
    /// subscribe to a message
    /// </summary>
    /// <param name="t">the type of message</param>
    /// <param name="e">the message to listen for</param>
    /// <param name="sub">the object that implements the interface</param>
    static public bool Subscribe<T>(MessageType t, string e, Callback<T> c, ISubscriber s)
    {
        string eventType = format(t, e);
        Subscriber<T> sub = new Subscriber<T>(t, e, c, s);

        object obj = sub as object;
        _subscribers.Add(obj);
       
        if (_eventTable.ContainsKey(eventType))
        {
            _eventTable[eventType] = (Callback<T>)_eventTable[eventType] + c;            
            return true;
        }

        _eventTable.Add(eventType, c);
        

        return true;
    }

    static public string format(MessageType t, string m)
    {
        return t.ToString().ToLower() + ":" + m.ToLower();
    }


    static public List<string> Subscribers
    {
        get
        {
            List<string> subsAsString = new List<string>();

            foreach (object o in _subscribers)
            {
                Subscriber s = o as Subscriber;

                if (s != null)
                    subsAsString.Add(s.SubscriberInfo);
            }

            return subsAsString;

        }
    }


    private class Subscriber
    {
        private MessageType type;
        private string message;
        private Callback callback;
        private ISubscriber sub;


        public Subscriber(MessageType t, string m, Callback c, ISubscriber s)
        {
            type = t;
            sub = s;
            callback = c;
            message = m;
        }

        public Callback Method { get { return callback; } }

        public string Message { get { return message.ToLower(); } }

        public string Name { get { return sub.ToString(); } }

        public string SubscriberInfo { get { return Name + ":" + format(type, message); } }

        public void Invoke() { callback(); }
    }

    private class Subscriber<T>
    {
        private MessageType type;
        private string message;
        private Callback<T> callback;
        private ISubscriber sub;

        public Subscriber(MessageType t, string m, Callback<T> c, ISubscriber s)
        {
            type = t;
            sub = s;
            callback = c;
            message = m;
        }

        public void Invoke(T arg) { callback(arg); }

        public string Message { get { return message.ToLower(); } }
        public string Name { get { return sub.ToString(); } }
        public string SubscriberInfo { get { return this.Name + ":" + format(type, message); } }


    }

}

