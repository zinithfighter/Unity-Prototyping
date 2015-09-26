using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 

public class EventSystem : Singleton<EventSystem> //must have monobehaviour or everyone will have to create an eventsystem object
{ 
    //a static reference to this eventsystem    
   Dictionary<string, ISubscriber> _subscribers;
    protected override void Awake()
    {
        base.Awake();
        _subscribers = new Dictionary<string, ISubscriber>();
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
        foreach (KeyValuePair<string, ISubscriber> s in _subscribers)
        {
            if (_subscribers.ContainsKey(message))
            {
                s.Value.Receive(message);
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
        _subscribers.Add(e, sub);
    }

    void RemoveSubscriber(string e, ISubscriber go)
    {

    }



}

