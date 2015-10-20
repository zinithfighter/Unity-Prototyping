using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;
using Party;
using System;


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
            Subscribe<string>(MessageLayer.GUI, "buttonclick", CombatEvents);
        }

        void Start()
        {
            Execute(OnCombatStart);
        }

        void CombatEvents(string input)
        {
            Execute(OnAbilitySelected);
        }

        void Execute(CombatAction action)
        {
            if (action != null)
                action();
        }

        void Execute<T>(CombatAction action, T arg)
        {
            Debug.Log("execute with " + arg.ToString());
            Delegate d = action;
            CombatAction<T> ca = d as CombatAction<T>;
            if (ca != null)
                ca(arg);
                
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