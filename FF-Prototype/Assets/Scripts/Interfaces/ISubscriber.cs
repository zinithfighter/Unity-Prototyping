using UnityEngine.EventSystems;
 
public interface ISubscriber : IEventSystemHandler
{   
    void Subscribe(MessageType t, string e, Callback c);  
}