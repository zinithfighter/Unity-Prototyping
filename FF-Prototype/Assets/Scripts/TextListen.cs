using UnityEngine;
using System.Collections;
using System;

public class TextListen : MonoBehaviour, ISubscriber {
    public string message;
    public MessageLayer layer;

    void Awake()
    {
        Subscribe<string>(layer, message, PopulateTextField);
    }

    public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
    {
        EventSystem.Subscribe<T>(t, e, c, this);
    }

    private void PopulateTextField(string info)
    {
        GetComponent<UnityEngine.UI.Text>().text = info;
    }
}
