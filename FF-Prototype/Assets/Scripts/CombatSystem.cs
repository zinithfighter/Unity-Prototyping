using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Combat
{
    public enum State
    {
        INIT, //setup gui
        START,
        ABILITY, //show gui
        TARGET, //hide gui
        RESOLVE,
        ENDTURN, //reset gui go to next party member
        EXIT,
    }

    public class CombatSystem : MonoBehaviour, IPublisher, ISubscriber
    {


        #region UnityEvents
        void Awake()
        {
            _fsm = new FiniteStateMachine<State>();

            _fsm.AddTransition(State.INIT, State.START, EnterStartHandler);
            _fsm.AddTransition(State.START, State.ABILITY, EnterAbilityHandler);
            _fsm.AddTransition(State.ABILITY, State.TARGET, EnterTargetHandler);
            _fsm.AddTransition(State.ABILITY, State.RESOLVE, EnterResolveHandler);
            _fsm.AddTransition(State.TARGET, State.ABILITY, EnterAbilityHandler);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, EnterResolveHandler);
            _fsm.AddTransition(State.RESOLVE, State.ABILITY, EnterAbilityHandler);
            _fsm.AddTransition(State.RESOLVE, State.EXIT, null);
            _states = new List<State>(_fsm.States);


            Subscribe(MessageLayer.GUI, "begin", StartToAbilityTrigger);
            Subscribe(MessageLayer.GUI, "attack", AbilityToTargetTrigger); //attack is clicked transition to targeting
            Subscribe(MessageLayer.GUI, "defend", AbilityToResolveTrigger); //endturn is clicked transition to start
            Subscribe(MessageLayer.GUI, "cancel", TargetToAbilityTrigger); //cancel is clicked transition to start  
            Subscribe(MessageLayer.GUI, "confirm", ResolveToAbilityTrigger);
            Subscribe(MessageLayer.GUI, "endturn", AbilityToResolveTrigger);


            Subscribe(MessageLayer.PARTY, "finished", ResolveToStartTrigger); //rotate a new party 

            _partyIndex = 0;
            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);

        }

        void Start() { InitToStartTrigger(); }



        void Update() { _currentState = CurrentState; }

        #endregion UnityEvents


        #region Handlers

        public void EnterStartHandler()
        {
            _currentParty = _combatParties[_partyIndex];
        }

        public void EnterAbilityHandler()
        {
            if (_currentParty == null)
                throw new System.NullReferenceException("Entering ability state but the current unit has not been assigned");
            if( _currentParty.CurrentUnit == null)
                throw new System.NullReferenceException("Entering ability state but the current unit has not been assigned");

        }

        public void EnterResolveHandler()//call resolve either from ability or target
        {

        }

        public void EnterTargetHandler() { }

        #endregion Handlers


        #region Triggers
        public void InitToStartTrigger() { _fsm.ChangeState(State.START); Publish(messageLayer, "StateChange", CurrentState); }
        //from start        
        public void StartToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); Publish(messageLayer, "StateChange", CurrentState); }
        //from ability
        public void AbilityToTargetTrigger() { _fsm.ChangeState(State.TARGET); Publish(messageLayer, "StateChange", CurrentState); }
        public void AbilityToResolveTrigger() { _fsm.ChangeState(State.RESOLVE); Publish(messageLayer, "StateChange", CurrentState); }
        //from target
        public void TargetToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); Publish(messageLayer, "StateChange", CurrentState); }
        public void TargetToResolveTrigger() { _fsm.ChangeState(State.RESOLVE); Publish(messageLayer, "StateChange", CurrentState); }
        //from resolve
        public void ResolveToStartTrigger() { _fsm.ChangeState(State.START); Publish(messageLayer, "StateChange", CurrentState); }
        public void ResolveToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); Publish(messageLayer, "StateChange", CurrentState); }
        public void ResolveToExitTrigger() { _fsm.ChangeState(State.EXIT); Publish(messageLayer, "StateChange", CurrentState); }
        #endregion Triggers

        #region Interface
        public void Publish(MessageLayer m, string e)
        {
            EventSystem.Broadcast(m, e);
        }

        public void Subscribe(MessageLayer t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }

        public void Publish<T>(MessageLayer m, string e, T args)
        {
            EventSystem.Broadcast<T>(m, e, args);
        }

        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }
        #endregion Interface
        public State CurrentState
        {
            get { return _fsm.currentState; }
        }


        public MessageLayer messageLayer = MessageLayer.COMBAT;

        private FiniteStateMachine<State> _fsm;

        [SerializeField]
        private List<CombatParty> _combatParties;

        [SerializeField]
        private CombatParty _currentParty;

        [SerializeField]
        int _partyIndex;

        [SerializeField]
        private State _currentState;

        [SerializeField]
        private List<State> _states;
    }


}