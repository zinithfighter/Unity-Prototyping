using UnityEngine;

public class PublishButton : MonoBehaviour, IPublisher
{
    MessageLayer messageLayer = MessageLayer.GUI;
    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PublishButtonClicked);
    }
    public void PublishButtonClicked()
    {
        Publish(messageLayer, "buttonclick", gameObject.name.ToLower());
    }

    public void PublishButtonHover()
    {
        Publish(messageLayer, "buttonhover", gameObject.name.ToLower());
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