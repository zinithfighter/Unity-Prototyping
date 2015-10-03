using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

 
public class UIRoot : MonoBehaviour, ISubscriber
{
    public GameObject _combatPanel;  
    
    void Awake()
    {
        
        Subscribe(MessageType.COMBAT, "init->start", EnableCombatPanel); 
        Subscribe(MessageType.COMBAT, "start->target", DisableCombatPanel); 
    }

    public void Subscribe(MessageType t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
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
