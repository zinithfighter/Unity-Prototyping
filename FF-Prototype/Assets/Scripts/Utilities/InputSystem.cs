using UnityEngine;
using System.Collections;
using System;

public class InputSystem : MonoBehaviour
    {
    


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EventSystem.Broadcast(MessageLayer.INPUT, "keydown", "escape");
        if (Input.GetKeyDown(KeyCode.Space))
            EventSystem.Broadcast(MessageLayer.INPUT, "keydown", "space");

    }
}
