using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {

    public Text label;
    public string log;
    void Update()
    {
        label.text = log;
    }

 
}
