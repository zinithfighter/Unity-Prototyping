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
        RESOLVE,
        EXIT,
    }



    public class CombatSystem : MonoBehaviour, IPublisher, ISubscriber
    {
        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, InitHandler);
            fsm.State(State.START, StartHandler);
            fsm.State(State.ACTIVE, ActiveHandler);
            fsm.State(State.RESOLVE, ResolveHandler);
            fsm.State(State.TARGET, TargetHandler);
            fsm.State(State.EXIT, ExitHandler);

            fsm.Transition(State.INIT, State.START, "start");
            fsm.Transition(State.START, State.ACTIVE, "space");
            fsm.Transition(State.ACTIVE, State.TARGET, "attack");
            fsm.Transition(State.TARGET, State.ACTIVE, "escape");
            fsm.Transition(State.TARGET, State.RESOLVE, "targetselected");
            fsm.Transition(State.ACTIVE, State.RESOLVE, "endturn");
            fsm.Transition(State.ACTIVE, State.RESOLVE, "defend");
            fsm.Transition(State.RESOLVE, State.EXIT, "quit");
            fsm.Transition(State.RESOLVE, State.ACTIVE, "space");
            UpdateFSM("*");
        }

        void Start()
        {
            UpdateFSM("start");
        }

        void UpdateFSM(string input)
        {
            Debug.Log("feed combat fsm with " + input);
            fsm.Feed(input);
        }

        void Shout(State state)
        {
            Publish(MessageLayer.COMBAT, "state change", state.ToString().ToLower());
        }

        void InitHandler()
        {
            Subscribe<string>(MessageLayer.INPUT, "key down", UpdateFSM);
            Subscribe<string>(MessageLayer.GUI, "buttonclick", UpdateFSM);
            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);
        }

        void StartHandler()
        {
            Shout(State.START);
        }

        void ActiveHandler()
        {
            Shout(State.ACTIVE);
        }

        void ResolveHandler()
        {
            if (_partyIndex >= _combatParties.Count)
            {
                _partyIndex = 0;
            }
            else
            {
                _partyIndex += 1;
                currentParty = _combatParties[0];
            }
        }

        void TargetHandler()
        {
            Shout(State.TARGET);
        }

        void ExitHandler()
        {
            Shout(State.EXIT);
        }
        
        #region Interface

        public void Publish<T>(MessageLayer m, string e, T args)
        {
            EventSystem.Broadcast(m, e, args);
        }

        public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
        {
            EventSystem.Subscribe(t, e, c, this);
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