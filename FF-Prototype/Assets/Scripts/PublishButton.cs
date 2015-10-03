using UnityEngine;
using System.Collections;
using System;

public class PublishButton : MonoBehaviour, IPublisher
{
    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PublishButtonClicked);
    }
    public void PublishButtonClicked()
    {
        Publish("gui:" + gameObject.name);
    }
    public void Publish(string e)
    {
        EventSystem.Broadcast(e);
    }
}
