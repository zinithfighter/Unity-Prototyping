using System;
using UnityEngine;

namespace Gui
{
    public class UIRoot : MonoBehaviour, ISubscriber
    {
        [SerializeField]
        private GameObject _combatPanel;

        [SerializeField]
        private GameObject _infoPanel;
        
        void Awake()
        {   
            Subscribe(MessageType.COMBAT, "init->start", OnStart);
           // Subscribe(MessageType.COMBAT, "start->target", OnTarget);

            //the generic template argument binds the signature of the delegate
            //allowing us to pass values to our delegate function we are subscribing to the event
            EventSystem.Subscribe(MessageType.COMBAT, "start->target", OnTarget, this);
            EventSystem.Subscribe<CombatUnit>(MessageType.COMBAT, "unit change", OnUnitChange, this);
        }

        public void Subscribe(MessageType t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }

        void OnStart()
        {
            _combatPanel.SetActive(true);
        }     

        void OnTarget()
        {
            _combatPanel.SetActive(false);            
        }

        void OnTargetSelected()
        {
            _combatPanel.SetActive(true);
        }

        void OnUnitChange(CombatUnit arg)
        {
            _infoPanel.GetComponent<UnityEngine.UI.Text>().text = 
                "Name: " + arg.name + 
                "\nHP: "+ arg.health +
                "\nAttack: "+ arg.attack +
                "\nDefense: " + arg.defense;
        }
    }
}
