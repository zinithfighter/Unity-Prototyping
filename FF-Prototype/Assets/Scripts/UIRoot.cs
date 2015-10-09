using System;
using UnityEngine;

namespace gui
{
    public class UIRoot : MonoBehaviour, ISubscriber
    {
        [SerializeField]
        private GameObject _combatPanel;

        [SerializeField]
        private GameObject _infoPanel;

        [SerializeField]
        private GameObject _confirmPanel;


        [SerializeField]
        private GameObject _beginPanel;

        void Awake()
        {   
            Subscribe(MessageType.COMBAT, "start", OnStart);
            Subscribe(MessageType.COMBAT, "ability", OnAbility);
            Subscribe(MessageType.COMBAT, "resolve", OnDefend);
           // Subscribe(MessageType.COMBAT, "start->target", OnTarget);

            //the generic template argument binds the signature of the delegate
            //allowing us to pass values to our delegate function we are subscribing to the event
            Subscribe(MessageType.COMBAT, "target", OnTarget);
            Subscribe<CombatUnit>(MessageType.COMBAT, "unit change", OnUnitChange);
            Subscribe(MessageType.GUI, "confirm", OnConfirm);
        }

        void Start()
        {
            _beginPanel.SetActive(false);

        } 
            

        public void Subscribe(MessageType t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }
        
        public void Subscribe<T>(MessageType t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }
        
        void OnConfirm()
        {
            _confirmPanel.SetActive(false);
        }
        void OnStart()
        {
            _beginPanel.SetActive(true);
            _combatPanel.SetActive(false);
            _confirmPanel.SetActive(false);
        }     

        void OnAbility()
        {
            _beginPanel.SetActive(false);
            _combatPanel.SetActive(true);
        }

        void OnTarget()
        {
            _combatPanel.SetActive(false);            
        }
        
        void OnDefend()
        {
            _combatPanel.SetActive(false);
            _confirmPanel.SetActive(true);
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
