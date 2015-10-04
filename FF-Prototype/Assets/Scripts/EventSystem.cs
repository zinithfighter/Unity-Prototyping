using UnityEngine;
using System.Collections;
using System.Collections.Generic;


static public class EventSystem
{


    /// <summary>
    /// Notify all the subscribers that a message has occurred
    /// </summary>
    /// <param name="message"></param>
    static public void Broadcast(MessageType m, string e)
    {
        string message = format(m, e);
        Debug.Log("Event Broadcast: " + message);
        Subscriber s;
        if (eventTable.TryGetValue(message, out s))
        {
            Debug.Log("execute " + message);
            s.Invoke();
        }
    }

    static private void RemoveSubscriber(ISubscriber go)
    {

    }

    static private List<Subscriber> _subscribers = new List<Subscriber>();

    static private Dictionary<string, Subscriber> eventTable = new Dictionary<string, Subscriber>();
    static public List<string> Subscribers
    {
        get
        {
            List<string> subsAsString = new List<string>();

            foreach (Subscriber s in _subscribers)
                subsAsString.Add(s.SubscriberInfo);

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
        foreach (Subscriber ss in _subscribers)
        {
            if (ss.SubscriberInfo == sub.SubscriberInfo)
                return false;
        }

        eventTable.Add(format(t, e), sub);
        _subscribers.Add(sub);

        return true;
    }

    static public string format(MessageType t, string m)
    {
        return t.ToString().ToLower() +":"+ m.ToLower();
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

        public string Message
        {
            get
            {
                return message.ToLower();
            }
        }

        public string Name
        {
            get
            {
                return sub.ToString();
            }
        }
        public string SubscriberInfo
        {
            get
            {
                return this.Name + ":" + format(type, message);
            }
        }

        public void Invoke()
        {
            callback();
        }

    }

}

