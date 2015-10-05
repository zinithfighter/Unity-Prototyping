using UnityEngine.EventSystems;

public interface IPublisher : IEventSystemHandler
{
    void Publish(MessageType m,  string e);
    void Publish<T>(MessageType m, string e, T args);
}
