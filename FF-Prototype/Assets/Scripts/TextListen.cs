 


public class TextListen : Observer
{
    public string message;
    public MessageLayer layer;

    void Awake()
    {
        Subscribe<string>(layer, message, PopulateTextField);
    }

    private void PopulateTextField(string info)
    {
        GetComponent<UnityEngine.UI.Text>().text = info;
    }
}
