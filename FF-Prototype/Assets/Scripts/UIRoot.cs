using System.Collections.Generic;
using UnityEngine;

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

        //[SerializeField]
        //private GameObject _confirmPanel;

        [SerializeField]
        private GameObject _infoPanel;

        [SerializeField]
        private GameObject _beginPanel;


        [SerializeField]
        private GameObject _phasePanel;

        [SerializeField]
        private UnityEngine.UI.Text _abilityInfo;

        [SerializeField]
        private UnityEngine.UI.Text _partyInfo;


        /// <summary>
        /// subscribe to combat and gui events then turn everything off
        /// </summary>
        void Awake()
        {
            Subscribe<string>(MessageLayer.COMBAT, "StateChange", OnCombatChange);
            Subscribe<string>(MessageLayer.UNIT, "StateChange", OnUnitChange);
            Subscribe<CombatUnit>(MessageLayer.UNIT, "UnitChange", OnUnitChange);

            _beginPanel.SetActive(false);
            //_confirmPanel.SetActive(false);
            _infoPanel.SetActive(false);
            _combatPanel.SetActive(false);
        }


        /// <summary>
        /// when the user hovers over a different ability 
        /// </summary>
        void OnAbilityChange(string arg)
        {
            _abilityInfo.text = arg;
        }

        void OnUnitChange(string state)
        {
            state = state.ToLower();
            //Debug.Log("combat state change " + state);
            switch (state)
            {
                case "init":
                    break;//setup gui
                case "start":
                    _phasePanel.SetActive(true);
                    _combatPanel.SetActive(false);
                    break;
                case "ability":
                    _phasePanel.SetActive(false);
                    _combatPanel.SetActive(true);
                    break;
                case "target":
                    break;
                case "resolve":
                    break;
                case "endturn":
                    break;
                case "exit":
                    break;
            }
        }

        /// <summary>
        /// when the unit changes update the info text
        /// </summary>
        /// <param name="arg"></param>
        void OnUnitChange(CombatUnit arg)
        {
            _partyInfo.text =
                "Name: " + arg.name +
                "\nHP: " + arg.health +
                "\nAttack: " + arg.attack +
                "\nDefense: " + arg.defense;
        }
        /// <summary>
        /// event listener for combat
        /// </summary>
        /// <param name="state"></param>
        void OnCombatChange(string state)
        {
            Debug.Log("combat state change " + state);
            state = state.ToLower();
            switch (state)
            {
                case "init":
                    _beginPanel.SetActive(false);
                    //_confirmPanel.SetActive(false);
                    _infoPanel.SetActive(false);
                    _combatPanel.SetActive(false);
                    break;//setup gui
                case "start":
                    _phasePanel.SetActive(true);
                    _combatPanel.SetActive(false);
                    break;
                case "active":
                    _phasePanel.SetActive(false);
                    _combatPanel.SetActive(true);
                    break;
                case "resolve":
                    break;

            }
        }




        public void Subscribe(MessageLayer t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }

        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }

    }
}
