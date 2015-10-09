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
        public MessageType messageLayer = MessageType.COMBAT;

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

            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);

            Subscribe(MessageType.GUI, "attack", AbilityToTargetTrigger); //attack is clicked transition to targeting
            Subscribe(MessageType.GUI, "defend", AbilityToResolveTrigger); //endturn is clicked transition to start
            Subscribe(MessageType.GUI, "cancel", TargetToAbilityTrigger); //cancel is clicked transition to start  
            Subscribe(MessageType.GUI, "confirm", ResolveToAbilityTrigger);
            Subscribe(MessageType.GUI, "endturn", AbilityToResolveTrigger);
            Subscribe(MessageType.PARTY, "finished", ResolveToStartTrigger); //rotate a new party 
            Subscribe(MessageType.GUI, "begin", StartToAbilityTrigger);


        }

        void Start()
        {
            _partyIndex = 0;
            InitToStartTrigger();
        }

        void Update()
        {
            _currentState = _fsm.currentState;
        }

        #endregion UnityEvents

 
        #region Handlers

        public void EnterStartHandler()
        {
            _currentParty = _combatParties[_partyIndex];
            _currentParty.StartParty();
            Publish(messageLayer, "start"); 
        }

        public void EnterAbilityHandler()
        {
            Publish(messageLayer, "ability"); //combat party is listening for this
        }

        public void EnterResolveHandler()
        {
            if (_partyIndex >= _combatParties.Count)
                _partyIndex = 0;
            else
            {
                _partyIndex += 1;
                ResolveToStartTrigger();
            }

            _currentParty.Next();
            Publish(messageLayer, "resolve");
        }

        public void EnterTargetHandler() { }

        #endregion Handlers

 
        #region Triggers
        public void InitToStartTrigger() { _fsm.ChangeState(State.START); }
        //from start        
        public void StartToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); }
        //from ability
        public void AbilityToTargetTrigger() { _fsm.ChangeState(State.TARGET); }
        public void AbilityToResolveTrigger() { _fsm.ChangeState(State.RESOLVE); }
        //from target
        public void TargetToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); }
        public void TargetToResolveTrigger() { _fsm.ChangeState(State.RESOLVE); }
        //from resolve
        public void ResolveToStartTrigger() { _fsm.ChangeState(State.START); }
        public void ResolveToAbilityTrigger() { _fsm.ChangeState(State.ABILITY); }
        public void ResolveToExitTrigger() { _fsm.ChangeState(State.EXIT); }
        #endregion Triggers

        #region Interface
        public void Publish(MessageType m, string e)
        {
            EventSystem.Broadcast(m, e);
        }

        public void Subscribe(MessageType t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }

        public void Publish<T>(MessageType m, string e, T args)
        {
            EventSystem.Broadcast<T>(m, e, args);
        }

        public void Subscribe<T>(MessageType t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }
        #endregion Interface

    }


}