using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;
using Party;


namespace Combat
{

    enum State
    {
        INIT,
        START,
        ACTIVE,
        TARGET,
        RESOLVE, //Determine if this party needs to be changed out
        ENDTURN,
        EXIT,
    }

    //responsible for intercepting gui input and relaying it
    public class CombatSystem : Observer
    {
        public delegate void CombatAction();
        public delegate void CombatAction<T>(T arg);
        public static event CombatAction OnCombatStart;
        public static event CombatAction OnAbilitySelected;

        void Awake()
        {
            Subscribe<string>(MessageLayer.GUI, "button clicked", CombatEvents);
        }

        void Start()
        {
            Execute(OnCombatStart);
        }

        void CombatEvents(string input)
        {
            Execute(OnAbilitySelected, input);
        }

        void Execute(CombatAction action)
        {
            if (action != null)
                action();
        }

        void Execute<T>(CombatAction<T> action, T arg)
        {
            if (action != null)
                action(arg);
        }

        #region Variables 

        [SerializeField]
        private List<CombatParty> _combatParties;

        [SerializeField]
        private CombatParty _currentParty;

        [SerializeField]
        private int _partyIndex;

        #endregion Variables
    }


}