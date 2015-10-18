using UnityEngine;

public class PublishButton : Observer
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



}