using UnityEngine.EventSystems;
 
public interface ISubscriber : IEventSystemHandler
{   
    void Subscribe(MessageLayer t, string e, Callback c);  
    void Subscribe<T>(MessageLayer t, string e, Callback<T> c); 
}