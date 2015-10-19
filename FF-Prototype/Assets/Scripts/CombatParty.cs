using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;
using Unit;

namespace Party
{ /// <summary>
  /// keeps track of all units in this group and manages them
  /// we will funnel all unit actions through this object before a notification
  /// is registered. We do this because i don't know
  /// </summary>
    enum State
    {
        INIT,
        START,
        ACTIVE,
        INACTIVE,
        TURN,
        EXIT,
    }

    public class CombatParty : Observer
    {

        public void Activate()
        {
            Debug.Log("activate party");
            UpdateFSM("activate");            
            _active = true;
        }

        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, InitHandler);
            fsm.State(State.START, StartHandler);
            fsm.State(State.INACTIVE, InactiveHandler);
            fsm.State(State.ACTIVE, ActiveHandler);
            fsm.State(State.TURN, TurnHandler);
            fsm.State(State.EXIT, ExitHandler);

            fsm.Transition(State.INIT, State.START, "start"); //subscribe to messages
            fsm.Transition(State.START, State.INACTIVE, "inactive"); //set all parties inactive
            fsm.Transition(State.INACTIVE, State.ACTIVE, "activate"); //begin animation on the first unit
            //manual activation from the combat system to start this particular party
            //if not activated from the combat system then all parties will be subscribing
            fsm.Transition(State.ACTIVE, State.TURN, "endturn"); //take turn when the combat fsm has moved to endturn
            fsm.Transition(State.TURN, State.ACTIVE, "active");  //turn->active when a unit has finished taking turn
            fsm.Transition(State.TURN, State.EXIT, "done"); //exit this fsm when all units have finished
            fsm.Transition(State.EXIT, State.INACTIVE, "inactive"); //move to inactive after finished
            UpdateFSM("*");
        }

        void OnStateChange(State state)
        {
            //Debug.Log("State change: Party: " + state.ToString().ToLower());
            Publish(MessageLayer.PARTY, "state change", state.ToString().ToLower() );
        }

        void UpdateFSM(string input)
        {
            //Debug.Log("feed party fsm with " + input);            
            fsm.Feed(input);
        }

        void InitHandler()
        {
            OnStateChange(State.INIT);
            //subscribe to combat state changes and give fsm the result
            Subscribe<string>(MessageLayer.COMBAT, "enter state", UpdateFSM); 
            _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(transform);
        }

        void StartHandler() //start of party
        {
            OnStateChange(State.START);            
            _currentUnit = _partyMembers[_unitIndex];
            UpdateFSM("inactive");
        }

        void InactiveHandler() //start of unit
        {
            _unitIndex = 0;
            turnsTaken = 0;
            OnStateChange(State.INACTIVE);                                   
        }

        void ActiveHandler() //start of unit
        {
            OnStateChange(State.ACTIVE);
            _currentUnit = _partyMembers[_unitIndex];
            _currentUnit.SetState(Unit.State.idle);
        }

        void TurnHandler()
        {
            OnStateChange(State.TURN);
            if (_unitIndex >= _partyMembers.Count - 1)
            {
                _currentUnit.SetState(Unit.State.ready);
                _currentUnit = null;
                UpdateFSM("done");
                return;
            }

            _currentUnit.SetState(Unit.State.idle);
            _unitIndex++; //increment the unit index  
            turnsTaken++;
        }

        void ExitHandler()
        {
            OnStateChange(State.EXIT);
            EventSystem.RemoveSubscriber(MessageLayer.COMBAT, "state change", this);            
            UpdateFSM("inactive");
        }



        #region Variables
        private FiniteStateMachine<State> fsm;
        /// <summary>
        /// the list of units participating in combat
        /// </summary>
        [SerializeField]
        private List<CombatUnit> _partyMembers;

        /// <summary>
        /// current index of the unit taking turn
        /// </summary>
        [SerializeField]
        private int _unitIndex;

        [SerializeField]
        private bool _active;


        /// <summary>
        /// the instance of the current unit taking turn
        /// </summary>
        [SerializeField]
        private CombatUnit _currentUnit;

        public CombatUnit CurrentUnit
        {
            get { return _currentUnit; }
        }
        /// <summary>
        /// how many turns we have taken
        /// </summary>
        public int turnsTaken;
        public string partyName
        {
            get { return name; }
            private set { partyName = name; }
        }
        /// <summary>
        /// number of members in the group
        /// </summary>    
        public int partySize
        {
            get { return _partyMembers.Count; }
        }

        #endregion Variables
    }
}