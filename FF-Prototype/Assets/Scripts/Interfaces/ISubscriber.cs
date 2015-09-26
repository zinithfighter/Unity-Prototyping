using UnityEngine;
using System.Collections;
 
public interface ISubscriber
{   
    void Subscribe(string t, string e);
    void Receive(string e); //receive an event as a string
}