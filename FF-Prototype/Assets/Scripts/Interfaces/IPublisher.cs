using UnityEngine;
using System.Collections;

public interface IPublisher
{
    void Publish(MessageType m,  string e);
}
