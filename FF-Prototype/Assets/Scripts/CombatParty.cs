using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;
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
        RESOLVE,
        EXIT,
    }

    public class CombatParty : MonoBehaviour, IPublisher, ISubscriber
    {                
        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, InitHandler);
            fsm.State(State.START, StartHandler);
            fsm.State(State.ACTIVE, ActiveHandler);
            fsm.State(State.RESOLVE, ResolveHandler);
            fsm.State(State.EXIT, ExitHandler);

            fsm.Transition(State.INIT, State.START, "start");
            fsm.Transition(State.START, State.ACTIVE, "activate");
            fsm.Transition(State.ACTIVE, State.RESOLVE, "resolve");
            fsm.Transition(State.RESOLVE, State.ACTIVE, "next");
            fsm.Transition(State.RESOLVE, State.EXIT, "dead");
            fsm.Transition(State.RESOLVE, State.START, "done");

            UpdateFSM("*");
        }

        void UpdateFSM(string input)
        {            
            fsm.Feed(input);
        }

        void Shout(State state)
        {
            Publish(MessageLayer.PARTY, "state change", state);
        }

        void InitHandler()
        {
            Shout(State.INIT);
            //subscribe to combat state changes and give fsm the result
            Subscribe<string>(MessageLayer.COMBAT, "state change", UpdateFSM);
            _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(transform);

        } 

        public void Activate()
        {
            UpdateFSM("activate");
        }

        void StartHandler()
        {
            Shout(State.START);
             
            _unitIndex = 0;            
            _currentUnit = _partyMembers[_unitIndex];
            _currentUnit.SetActive(false);
        }

        void ActiveHandler()
        {
            Shout(State.ACTIVE);                        
            _currentUnit = _partyMembers[_unitIndex];
            _currentUnit.SetActive(true);
            
        } 

        void ResolveHandler()
        {
            Shout(State.RESOLVE);
            if (_unitIndex >= _partyMembers.Count - 1)
            {                
                _unitIndex = 0;
                _currentUnit = _partyMembers[_unitIndex];
                _currentUnit.SetActive(false);
                UpdateFSM("done");
            }
            else
            {
                _currentUnit.SetActive(false);
                _unitIndex += 1; //increment the unit index  
                turnsTaken += 1;
                UpdateFSM("next");
            }
        }

        void ExitHandler()
        {
            Shout(State.EXIT);
            _unitIndex = 0;
        }
                
        #region Interfaces
        public void Publish(MessageLayer m, string e)
        {
            EventSystem.Broadcast(m, e);
        }

        public void Publish<T>(MessageLayer m, string e, T args)
        {
            EventSystem.Broadcast<T>(m, e, args);
        }

        public void Subscribe(MessageLayer t, string e, Callback c)
        {
            EventSystem.Subscribe(t, e, c, this);
        }

        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe<T>(t, e, c, this);
        }
        #endregion Interfaces
        
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