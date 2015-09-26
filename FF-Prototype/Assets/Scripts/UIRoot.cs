using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

 
public class UIRoot : MonoBehaviour, ISubscriber
{
    public GameObject _combatPanel; 

    public delegate void UIHandler();
    UIHandler OnStart, OnTarget, OnResolve, OnEndTurn, OnExit;
    
    void Awake()
    { 
        Subscribe("combat", "init->start");
        OnStart += DisableCombatPanel;

        Subscribe("combat", "start->target");
        OnTarget += EnableCombatPanel;
    }

    public void Subscribe(string t, string e)
    {
        EventSystem.AddSubscriber(t, e, this);
    }

    public void Receive(string e)
    { 
        Debug.Log("GUI Received EVENT! " + e);

        switch(e)
        {
            case "combat:init->start":
                OnStart();
                break;
            case "combat:start->target":
                OnTarget();
                break;          
            default:
                break;
        }
    } 
     
    void DisableCombatPanel()
    {
        _combatPanel.SetActive(false);
    }

    void EnableCombatPanel()
    {
        _combatPanel.SetActive(true);
    } 
  
   
}
