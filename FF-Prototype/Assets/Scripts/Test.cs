using UnityEngine;
using System.Collections;
using System;

public class Test : MonoBehaviour, IPublisher, ISubscriber
{
    public UnityEngine.UI.Text textField;
    public GameObject PartyTakingTurn;
    public Combat.CombatSystem combats;
    void Awake()
    {
        print("doit in awake");
    }
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
        doit();
    }


    public void doit()
    {
        print("doit");
    }

    #region interfaces
    public void Publish(MessageLayer m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Subscribe(MessageLayer t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }


    public void Publish<T>(MessageLayer m, string e, T args)
    {
        EventSystem.Broadcast<T>(m, e, args);
    }

    public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
    {
        EventSystem.Subscribe<T>(t, e, c, this);
    }
    #endregion interfaces

    public delegate bool StateTrigger();

    StateTrigger IdleTriggers;

    IEnumerator CheckDistance()
    {
        yield return null;
    }
}
