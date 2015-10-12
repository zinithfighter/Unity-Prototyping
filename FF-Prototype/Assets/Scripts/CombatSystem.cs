using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine;
using System;

namespace Combat
{
    public enum State
    {
        INIT, //setup gui
        START,
        ABILITY,
        TARGET,
        RESOLVE,
        ENDTURN, //reset gui go to next party member
        EXIT,
    }

    public class CombatSystem : MonoBehaviour, IPublisher, ISubscriber
    {
        #region UnityEvents
        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, StateHandler);
            fsm.State(State.START, StateHandler);
            fsm.State(State.ABILITY, StateHandler);
            fsm.State(State.TARGET, StateHandler);
            fsm.State(State.RESOLVE, StateHandler);
            fsm.State(State.ENDTURN, StateHandler);
            fsm.State(State.EXIT, StateHandler);

            fsm.Transition(State.INIT, State.START, "*");
            fsm.Transition(State.START, State.ABILITY, "begin");
            fsm.Transition(State.ABILITY, State.TARGET, "attack"); 
            fsm.Transition(State.TARGET, State.ABILITY, "escape");
            fsm.Transition(State.TARGET, State.ABILITY, "confirm");
            fsm.Transition(State.TARGET, State.RESOLVE, "enemy-selected");
            fsm.Transition(State.ABILITY, State.RESOLVE, "endturn");
            fsm.Transition(State.RESOLVE, State.ENDTURN, "confirm");
            fsm.Transition(State.ENDTURN, State.ABILITY, "unitdone");
            fsm.Transition(State.ENDTURN, State.START, "partydone");
            fsm.Transition(State.ENDTURN, State.EXIT, "unitdone");

            Subscribe<string>(MessageLayer.INPUT, "keydown", UpdateFSM);
            Subscribe<string>(MessageLayer.GUI, "button->click", UpdateFSM);
            Subscribe<string>(MessageLayer.PARTY, "finished", UpdateFSM); //rotate a new party 

            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);

            
        }
        void Start()
        {
            UpdateFSM("*");
        }
        #endregion UnityEvents
        void Update()
        {
            Debug.Log(fsm.CurrentState);
        }
        void UpdateFSM(string input)
        {
            Debug.Log("feed fsm with " + input);
            fsm.Feed(input);

        }
        void StateHandler()
        {
            string state = fsm.CurrentState.ToString().ToLower();
            Publish(MessageLayer.COMBAT, "StateChange", state);
            Debug.Log("current state is " + state);
        }

        void OnPartyFinished() { }
        void OnConfirm() { }
        void OnCancel() { }



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


        public MessageLayer messageLayer = MessageLayer.COMBAT;

        private FiniteStateMachine<State> fsm;

        [SerializeField]
        private List<CombatParty> _combatParties;

        static public CombatParty currentParty
        {
            get;
            private set;
        }


        #endregion variables
    }


}