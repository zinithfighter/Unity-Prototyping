using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
using System;

namespace Combat
{
    enum State
    {
        INIT,
        START,
        ACTIVE,
        RESOLVE,
        EXIT,
    }

    public class CombatSystem : MonoBehaviour, IPublisher, ISubscriber
    {
        #region UnityEvents
        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, StateChange);
            fsm.State(State.START, StartHandler);
            fsm.State(State.ACTIVE, StateChange);
            fsm.State(State.RESOLVE, ResolveHandler);
            fsm.State(State.EXIT, StateChange);

            fsm.Transition(State.INIT, State.START, "*");
            fsm.Transition(State.START, State.ACTIVE, "space");
            fsm.Transition(State.ACTIVE, State.RESOLVE, "resolve");
            fsm.Transition(State.RESOLVE, State.ACTIVE, "next");
            fsm.Transition(State.RESOLVE, State.EXIT, "done");

            Subscribe<string>(MessageLayer.PARTY, "finished", UpdateFSM); //rotate a new party 
            Subscribe<string>(MessageLayer.INPUT, "keydown", UpdateFSM); //rotate a new party 
            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);


        }

        void StateChange()
        {
            string currentState = fsm.CurrentState.ToString();
            Publish(MessageLayer.COMBAT, "StateChange", currentState);
        }


        void UpdateFSM(string input)
        {
            Debug.Log("feed combat fsm with " + input);
            fsm.Feed(input);
        }
        #endregion UnityEvents
        void Start()
        {
            UpdateFSM("*");
        }

        void StartHandler()
        {
            StateChange();
            currentParty = _combatParties[0];

        }

        void ResolveHandler()
        {
            if (_partyIndex >= _combatParties.Count)
                _partyIndex = 0;
            else
            {
                _partyIndex += 1;
                currentParty = _combatParties[0];
            }

        }


        #region Interface

        public void Publish<T>(MessageLayer m, string e, T args)
        {
            EventSystem.Broadcast<T>(m, e, args);
        }

        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }

        public void Publish(MessageLayer m, string e)
        {
            EventSystem.Broadcast(m, e);
        }
        #endregion Interface



        #region variables


        private FiniteStateMachine<State> fsm;

        [SerializeField]
        private List<CombatParty> _combatParties;

        static public CombatParty currentParty
        {
            get;
            private set;
        }

        int _partyIndex;

        #endregion variables
    }


}