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
            fsm.State(State.TARGET, TargetHandler);
            fsm.State(State.RESOLVE, ResolveHandler);
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

        void OnStateChange(State state)
        {
            Debug.Log("State change: Combat: " + state.ToString().ToLower());
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
            OnStateChange(State.START);
            _partyIndex = 0;
            currentParty = _combatParties[_partyIndex];
            currentParty.Activate();
        }

        void ActiveHandler()
        {
            OnStateChange(State.ACTIVE);            
            
        }

        void ResolveHandler()
        {
            OnStateChange(State.RESOLVE);
            if (_partyIndex >= _combatParties.Count - 1)
            {
                _partyIndex = 0;
            }
            else
            {
                _partyIndex += 1;
                currentParty = _combatParties[_partyIndex];
            }
        }

        void TargetHandler()
        {
            OnStateChange(State.TARGET);
        }

        void ExitHandler()
        {
            OnStateChange(State.EXIT);
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

        [SerializeField]
        private CombatParty currentParty;

         

        int _partyIndex;

        #endregion variables
    }


}