using UnityEngine;
using System.Collections;
 
public interface ISubscriber
{   
    void Subscribe(MessageType t, string e, Callback c); 
}