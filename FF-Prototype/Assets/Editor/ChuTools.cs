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
        // The actual window code goes here
        EditorGUILayout.BeginHorizontal();        
            EditorGUILayout.BeginVertical();
                GUILayout.Label("Subscriber");
                for (int i = 0; i < EventSystem.Subscribers.Count; i++)
                {

                    string info = EventSystem.Subscribers[i];
                    string[] info2 = info.Split(':');

                    EditorGUILayout.LabelField(info2[0]);

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                GUILayout.Label("Type");
                for (int i = 0; i < EventSystem.Subscribers.Count; i++)
                {

                    string info = EventSystem.Subscribers[i];
                    string[] info2 = info.Split(':');

                    EditorGUILayout.LabelField(info2[1]);

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                GUILayout.Label("Message");
                for (int i = 0; i < EventSystem.Subscribers.Count; i++)
                {

                    string info = EventSystem.Subscribers[i];
                    string[] info2 = info.Split(':');
                    EditorGUILayout.LabelField(info2[2]);


                }

            EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

    }
}



