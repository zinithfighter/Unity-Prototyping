using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour {


    void Awake()
    {
        transform.parent = GameObject.Find("Buttons").transform;

    }
}
