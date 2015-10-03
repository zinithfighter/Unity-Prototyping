using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

class Subscriber : EditorWindow
{
    [MenuItem("ChuTools/Subscribers")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Subscriber));
    }



    void OnGUI()
    {
        int size = EventSystem.Subscribers.Count;
        string[] names = new string[size];
        string[] types = new string[size];
        string[] messages = new string[size];

        for (int i = 0; i < EventSystem.Subscribers.Count; i++)
        {
            string info = EventSystem.Subscribers[i];
            string[] infoSplit = info.Split(':');
            names[i] = infoSplit[0];
            types[i] = infoSplit[1];
            messages[i] = infoSplit[2];
        }

        EditorGUILayout.BeginHorizontal();


        Setup(1, ref names, "Subscriber"); 
        Setup(2, ref types, "Type"); 
        Setup(3, ref messages,"Message");

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




