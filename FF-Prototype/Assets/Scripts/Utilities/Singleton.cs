using UnityEngine;
using System.Collections;


/// <summary>
/// generic singleton class
/// usage: inherit from this class and pass in the name of the class that is inheriting
/// example: Gamemanager : Singleton<GameManager>
/// </summary>
/// <typeparam name="T"></typeparam>
/// 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance
    { //Gamemanager.instance.
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //if the instance is not assigned
        if (_instance == null)
        {
            //then grab the T from the gameobject this script is attached to
            //since we are inheriting why we no get it?
            _instance = this.gameObject.GetComponent<T>();
            DontDestroyOnLoad(_instance);
        }
        else
        {
            if (this != _instance)
            {
                print("found another instance of ID: " + gameObject.GetInstanceID() + ". Destroying.");
                Destroy(this.gameObject);

            }
        }
    }
}