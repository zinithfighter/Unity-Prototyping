using UnityEngine;
using System.Collections;
using System;

namespace gui
{
    public class PublishButton : MonoBehaviour, IPublisher
    {
        MessageType messageLayer = MessageType.GUI;
        void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PublishButtonClicked);
        }
        public void PublishButtonClicked()
        {
            Publish(messageLayer, gameObject.name);
        }
        public void Publish(MessageType m, string e)
        {
            EventSystem.Broadcast(m, e);
        }
    }
}