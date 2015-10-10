using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// if you want to use this a couple of things will need to be setup
/// you will need a way to get all of your subscribers as a single string value
/// and they must be seperated in the following format
/// <object>:<type>:<message>
/// </summary>


class Subscriber : EditorWindow
{

    /// <summary>
    /// draw a window 
    /// </summary>
    [MenuItem("ChuTools/Subscribers")]
    public static void ShowWindow()
    {
        //returns the first instance of this window of type which is this class
        GetWindow(typeof(Subscriber));
    }

    void OnGUI()
    {

        int size = EventSystem.Subscribers.Count;
        string[] names = new string[size];
        string[] types = new string[size];
        string[] messages = new string[size];

        for (int i = 0; i < EventSystem.Subscribers.Count; i++)//split the string into three substrings name:type:message
        {
            string info = EventSystem.Subscribers[i];
            string[] infoSplit = info.Split(':');
            names[i] = infoSplit[0];
            types[i] = infoSplit[1];
            messages[i] = infoSplit[2];
        }

        EditorGUILayout.BeginHorizontal();


        Setup(1, ref names, "Subscriber (Namespace.Class)");
        Setup(2, ref types, "Type");
        Setup(3, ref messages, "Message");

        EditorGUILayout.EndHorizontal();


    }

    void Setup(int col, ref string[] s, string name)
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Label(name);

        for (int i = 0; i < EventSystem.Subscribers.Count; i++)
        {
            EditorGUILayout.LabelField(s[i]);
        }

        EditorGUILayout.EndVertical();

    }
}




