using UnityEngine;
using System.Collections;
using System;

public class Test : MonoBehaviour, IPublisher, ISubscriber
{
    public UnityEngine.UI.Text textField;
    public GameObject PartyTakingTurn;
    public Combat.CombatSystem combats;

    void Start()
    {
        //textField.text = PartyTakingTurn.name;
        foreach (Transform u in PartyTakingTurn.transform)
        {
            if (u.GetComponent<IUnit>() != null)
            {
                string att = u.GetComponent<IUnit>().attack.ToString();
                string hp = u.GetComponent<IUnit>().health.ToString();
                string def = u.GetComponent<IUnit>().defense.ToString(); 
            }
        } 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Publish(MessageType.COMBAT, "TurnFinished");
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            Publish(MessageType.COMBAT, "init->start");            
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Subscribe(MessageType.PARTY, "doit", doit);
        }
    }

    public void doit()
    {
        print("doit");
    }

    public void Publish(MessageType m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Subscribe(MessageType t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }


    public void Publish<T>(MessageType m, string e, T args)
    {
        EventSystem.Broadcast<T>(m, e, args);
    }

    public void Subscribe<T>(MessageType t, string e, Callback<T> c)
    {
        EventSystem.Subscribe<T>(t, e, c, this);
    }
}
