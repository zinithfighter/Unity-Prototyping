using UnityEngine;
using System.Collections;
using System;

public class Test : MonoBehaviour, IPublisher
{
    public UnityEngine.UI.Text textField;
    public GameObject PartyTakingTurn;
    public Combat.CombatSystem combats;

    void Start()
    {
        textField.text = PartyTakingTurn.name;
        foreach (Transform u in PartyTakingTurn.transform)
        {
            if (u.GetComponent<IUnit>() != null)
            {
                string att = u.GetComponent<IUnit>().attack.ToString();
                string hp = u.GetComponent<IUnit>().health.ToString();
                string def = u.GetComponent<IUnit>().defense.ToString();
               // Debug.Log(u.name + "'s attack is " + att);
               // Debug.Log(u.name + "'s health is " + hp);
               // Debug.Log(u.name + "'s defense is " + def);
            }
        } 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Publish("init->start");            
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            combats.ChangeState(Combat.State.TARGET);    
        }
    }

    public void Publish(string e)
    {
        EventSystem.Broadcast(e);
    }
}
