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
            _states = new List<State>(_fsm.States);

            _fsm.AddTransition(State.INIT, State.START, EnterStart, ExitState);
            _fsm.AddTransition(State.START, State.ABILITY, EnterAbility, ExitState);

            _fsm.AddTransition(State.ABILITY, State.TARGET, EnterTarget, ExitState);
            _fsm.AddTransition(State.ABILITY, State.RESOLVE, EnterResolve, ExitState);

            _fsm.AddTransition(State.TARGET, State.ABILITY, EnterAbility, ExitState);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, EnterResolve, ExitState);

            _fsm.AddTransition(State.RESOLVE, State.START, EnterStart, ExitState);
            _fsm.AddTransition(State.RESOLVE, State.ABILITY, EnterAbility, ExitState);
            _fsm.AddTransition(State.RESOLVE, State.EXIT, null, null);

            Subscribe(MessageLayer.GUI, "begin", AbilityTrigger);
            Subscribe(MessageLayer.GUI, "cancel", OnCancel); //cancel is clicked transition to start  
            Subscribe(MessageLayer.GUI, "endturn", ResolveTrigger);
            Subscribe(MessageLayer.PARTY, "finished", StartTrigger); //rotate a new party 

            _partyIndex = 0;
            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);

        }

        void Start() { StartTrigger(); }



        void Update() { _currentState = CurrentState; }

        #endregion UnityEvents



        #region s

        void ExitState()
        {
            Debug.Log("Exiting " + CurrentState.ToString());
 
        }

        void EnterStart()
        {
            Debug.Log("Entering " + CurrentState.ToString());
 
            CurrentParty = _combatParties[_partyIndex];
        }

        void EnterAbility()
        {
            Debug.Log("Entering " + CurrentState.ToString());
            if (_currentParty == null)
                throw new System.NullReferenceException("Entering ability state but the current unit has not been assigned");
            if (_currentParty.CurrentUnit == null)
                throw new System.NullReferenceException("Entering ability state but the current unit has not been assigned");
        }

        public void EnterResolve()
        {
            if (CurrentParty.turnsTaken > CurrentParty.partySize)
            {
                Debug.Log("NEW PARTY TURN");
                _partyIndex += 1;
            }
        }

        void OnConfirm()
        {
            if (CurrentParty.turnsTaken <= CurrentParty.partySize)
                _fsm.ChangeState(State.ABILITY);
        }

        void OnCancel()
        {
            _fsm.ReverseState();
        }




        public void EnterTarget() { }

        #endregion s


        #region Triggers
        public void StartTrigger() { _fsm.ChangeState(State.START); Publish(messageLayer, "StateChange", CurrentState); }
        //from start        
        public void AbilityTrigger() { _fsm.ChangeState(State.ABILITY); Publish(messageLayer, "StateChange", CurrentState); }
        //from ability
        public void TargetTrigger() { _fsm.ChangeState(State.TARGET); Publish(messageLayer, "StateChange", CurrentState); }
        public void ResolveTrigger() { _fsm.ChangeState(State.RESOLVE); Publish(messageLayer, "StateChange", CurrentState); }
        void RestartTrigger()
        {
            _fsm.ChangeState(State.START);
        }

        //from target


        //from resolve


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



        #region variables
        public State CurrentState
        {
            get { return _fsm.currentState; }
        }


        public MessageLayer messageLayer = MessageLayer.COMBAT;

        private FiniteStateMachine<State> _fsm;

        [SerializeField]
        private List<CombatParty> _combatParties;

        [SerializeField]
        static private CombatParty _currentParty;


        static public CombatParty CurrentParty
        {
            get
            {
                return _currentParty;
            }
            private set
            {
                _currentParty = value;
            }

        }

        [SerializeField]
        int _partyIndex;

        [SerializeField]
        private State _currentState;

        [SerializeField]
        private List<State> _states;
        #endregion variables
    }


}