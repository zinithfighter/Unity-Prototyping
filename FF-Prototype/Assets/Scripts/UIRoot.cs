using System;
using UnityEngine;
using Combat;

namespace gui
{
    public enum UIEvents
    {
        HOVER,
        CLICK,
    }
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
            Subscribe<string>(MessageLayer.COMBAT, "StateChange", OnCombatStateChange);
            Subscribe<CombatUnit>(MessageLayer.UNIT, "UnitChange", OnUnitChange);
            //the generic template argument binds the signature of the delegate
            //allowing us to pass values to our delegate function we are subscribing to the event
            Subscribe(MessageLayer.COMBAT, "target", OnTarget);            
            Subscribe(MessageLayer.GUI, "confirm", OnConfirm);
            Subscribe(MessageLayer.GUI, "cancel", OnCancel);
        }

        /// <summary>
        /// event listener for combat
        /// </summary>
        /// <param name="state"></param>
        void OnCombatStateChange(string state)
        {
            Debug.Log("combat state change " + state);
            switch (state)
            {
                case "init":
                    break;//setup gui
                case "start":
                    _beginPanel.SetActive(true);
                    _combatPanel.SetActive(false);
                    _confirmPanel.SetActive(false);
                    break;
                case "ability":
                    _beginPanel.SetActive(false);
                    _combatPanel.SetActive(true);
                    break;
                case "target":
                    break;
                case "resolve":
                    _combatPanel.SetActive(false);
                    _confirmPanel.SetActive(true);
                    break;
                case "exit":
                    break;
            }
        }

        void OnConfirm()
        {
            _confirmPanel.SetActive(false);
        }

        void OnCancel()
        {
            _confirmPanel.SetActive(false);
        }

        void Start()
        {
            _beginPanel.SetActive(false);
        }             

        public void Subscribe(MessageLayer t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }
        
        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }
 
        void OnTarget()
        {
            _combatPanel.SetActive(false);            
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
