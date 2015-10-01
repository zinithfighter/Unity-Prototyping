using UnityEngine;
using System.Collections;
using System;

public class Relay : MonoBehaviour, ISubscriber
{
    public enum MessageType
    {
        combat,
        gui,
    }
    public MessageType _messageType;
    public string message;
    public void Start()
    {
        Subscribe(_messageType.ToString(), message);
    }
    public void Receive(string e)
    {
        if (e == _messageType.ToString() + ":" + message) 
            Debug.Log("got it");
    }

    public void Subscribe(string t, string e)
    {
        EventSystem.AddSubscriber(t, e, this);
    }
}
