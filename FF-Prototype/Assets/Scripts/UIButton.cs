using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIButton : MonoBehaviour
{
#if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying == false)
            Rename();

    }

#endif

    void Rename()
    {

        Text _text = GetComponentInChildren<Text>();
        if (_text.text != name)
        {
            _text.text = name;
        }

    }
}