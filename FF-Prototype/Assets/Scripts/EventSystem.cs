using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EventSystem : Singleton<EventSystem> //must have monobehaviour or everyone will have to create an eventsystem object
{
    private class Listener
    {
        
        public string _event;
        public ISubscriber _sub;
        
        public Listener()
        {

        }

        public Listener(string e, ISubscriber s)
        {
            this._event = e;
            this._sub = s;
        }
        
    }
    //a static reference to this eventsystem    
    //key is the event
    //value is the subscriber

    private List<Listener> _subscribers = new List<Listener>();
    protected override void Awake()
    {
        base.Awake();
         
    }
 
    //if anyone is subscribed to that message
    //then notify them that the message happened
    static public void Notify(string message)
    {
        Debug.Log("Event Broadcast: " + message);
        instance.NotifySubs(message);        
    }

    private void NotifySubs(string message)
    {        
        foreach (Listener l in _subscribers)
        {
            if (l._event == message)
            {
                Debug.Log("notify subs " + l._event);
                l._sub.Receive(message);
            }
        }
    }

    /// <summary>
    /// subscribe to a message
    /// </summary>
    /// <param name="t">the type of message</param>
    /// <param name="e">the message to listen for</param>
    /// <param name="sub">the object that implements the interface</param>
    static public void AddSubscriber(string t, string e, ISubscriber sub)
    {
        string type = t;
        string message = e;
        string subscription = type + ":" + message;
        instance.AddListener(subscription, sub);
    }

    private void AddListener(string e, ISubscriber sub)
    {
        
        Listener l = new Listener(e, sub); 
        _subscribers.Add(l);
        Debug.Log("add listener " + e);


    }

    void RemoveSubscriber(string e, ISubscriber go)
    {

    }



}

