using UnityEngine;
using System.Collections;
using System;

namespace gui
{
    public class PublishButton : MonoBehaviour, IPublisher
    {
        MessageLayer messageLayer = MessageLayer.GUI;
        void Awake()
        {
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PublishButtonClicked);
        }
        public void PublishButtonClicked()
        {
            Publish(messageLayer, gameObject.name);
        }
        public void Publish(MessageLayer m, string e)
        {
            EventSystem.Broadcast(m, e);
        }


        public void Publish<T>(MessageLayer m, string e, T args)
        {
            EventSystem.Broadcast<T>(m, e, args);
        }

        
    }
}