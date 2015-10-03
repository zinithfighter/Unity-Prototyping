using UnityEngine;
using System.Collections;
using System.Collections.Generic;


static public class EventSystem
{


    /// <summary>
    /// Notify all the subscribers that a message has occurred
    /// </summary>
    /// <param name="message"></param>
    static public void Broadcast(string message)
    {
        Debug.Log("Event Broadcast: " + message);
    } 

    static private void RemoveSubscriber(ISubscriber go)
    {

    }

    static private List<Subscriber> _subscribers = new List<Subscriber>();

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
    static public void Subscribe(MessageType t, string e, Callback c, ISubscriber s)
    {


        Subscriber sub = new Subscriber(t, e, c, s);
        _subscribers.Add(sub);


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

        public string SubscriberInfo
        {
            get
            {
                return sub.ToString() + ":" + type.ToString() +":" + message.ToString();
            }
        }
    }

}

