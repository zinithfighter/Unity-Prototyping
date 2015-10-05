using UnityEngine.EventSystems;

public interface IPublisher : IEventSystemHandler
{
    void Publish(MessageType m,  string e);
}
