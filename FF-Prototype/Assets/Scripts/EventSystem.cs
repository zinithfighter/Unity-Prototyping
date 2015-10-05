using UnityEngine;
using System.Collections;
using System.Collections.Generic;


static public class EventSystem
{
    static private List<object> _subscribers = new List<object>();

    static private Dictionary<string, object> _eventTable = new Dictionary<string, object>();

    /// <summary>
    /// Notify all the subscribers that a message has occurred
    /// </summary>
    /// <param name="message"></param>
    static public void Broadcast(MessageType m, string e)
    {
        string message = format(m, e);
        Debug.Log("Event Broadcast: " + message);
        object o;
        if (_eventTable.TryGetValue(message, out o))
        {
            //Debug.Log("execute " + message);
            Subscriber s = o as Subscriber;
            if (s != null)
                s.Invoke();

        }
    }

    static public void Broadcast<T>(MessageType m, string e, T arg)
    {
        string message = format(m, e);
        Debug.Log("Event Broadcast: " + message);
        object o;
        if (_eventTable.TryGetValue(message, out o))
        {
            //Debug.Log("execute " + message);
            Subscriber<T> s = o as Subscriber<T>;
            if (s != null)
                s.Invoke(arg);
        }
    }

    static private void RemoveSubscriber(ISubscriber go)
    {

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

    /// <summary>
    /// subscribe to a message
    /// </summary>
    /// <param name="t">the type of message</param>
    /// <param name="e">the message to listen for</param>
    /// <param name="sub">the object that implements the interface</param>
    static public bool Subscribe(MessageType t, string e, Callback c, ISubscriber s)
    {
        Subscriber sub = new Subscriber(t, e, c, s);
        foreach (object o in _subscribers)
        {
            Subscriber ss = o as Subscriber;
            if (ss != null)
            {
                if (ss.SubscriberInfo == sub.SubscriberInfo)
                    return false;
            }
        }

        _eventTable.Add(format(t, e), sub);
        _subscribers.Add(sub);

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
        Subscriber<T> sub = new Subscriber<T>(t, e, c, s);
        foreach (object o in _subscribers)
        {
            Subscriber ss = o as Subscriber;
            if (ss.SubscriberInfo == sub.SubscriberInfo)
                return false;
        }

        _eventTable.Add(format(t, e), sub);
        _subscribers.Add(sub);

        return true;
    }

    static public string format(MessageType t, string m)
    {
        return t.ToString().ToLower() + ":" + m.ToLower();
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

        public string Message { get { return message.ToLower(); } }

        public string Name { get { return sub.ToString(); } }

        public string SubscriberInfo { get { return this.Name + ":" + format(type, message); } }

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

