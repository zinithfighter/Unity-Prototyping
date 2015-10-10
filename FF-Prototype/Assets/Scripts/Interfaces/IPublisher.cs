using UnityEngine.EventSystems;

public interface IPublisher : IEventSystemHandler
{
    void Publish(MessageLayer m,  string e);
    void Publish<T>(MessageLayer m, string e, T args);
}
