using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IBehaviour : IEventSystemHandler
{
    void Enable();
    void Disable();
}
