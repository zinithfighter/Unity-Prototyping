using UnityEngine.EventSystems;
 
public interface ISubscriber 
{   
    void Subscribe<T>(MessageLayer t, string e, Callback<T> c); 
}