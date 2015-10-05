using UnityEngine.EventSystems;
 
public interface ISubscriber : IEventSystemHandler
{   
    void Subscribe(MessageType t, string e, Callback c);  
    void Subscribe<T>(MessageType t, string e, Callback<T> c);  
}