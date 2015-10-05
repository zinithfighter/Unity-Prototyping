using UnityEngine;
using System.Collections.Generic;

namespace Combat
{


    public enum State
    {
        INIT, //setup gui
        START, //show gui
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
        private State _currentState;

        void Awake()
        {
            _fsm = new FiniteStateMachine<State>();
            _fsm.AddTransition(State.INIT, State.START, InitToStartHandler);
            _fsm.AddTransition(State.START, State.TARGET, StartToTargetHandler);
            _fsm.AddTransition(State.START, State.ENDTURN, StartToEndTurnHandler);
            _fsm.AddTransition(State.TARGET, State.START, TargetToStartHandler);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, TargetToResolveHandler);
            _fsm.AddTransition(State.RESOLVE, State.ENDTURN, ResolveToEndTurnHandler);
            _fsm.AddTransition(State.ENDTURN, State.EXIT, EndTurnToExitHandler);
            _fsm.AddTransition(State.ENDTURN, State.START, EndTurnToStartHandler);

            _fsm.Begin(State.INIT);

            Subscribe(MessageType.GUI, "attack", TargetTrigger);
            Subscribe(MessageType.GUI, "cancel", StartTrigger);
            Subscribe(MessageType.GUI, "endturn", EndTurnTrigger);

            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(this.transform);
        }

        void Start()
        {
            _fsm.ChangeState(State.START);
        }

        void Update()
        {
            _currentState = _fsm.currentState;
        }

        #region Handlers
        public void InitToStartHandler()
        {
            int partyIndex = Random.Range(1, _combatParties.Count);
            _currentParty = _combatParties[partyIndex];
            _currentParty.active = true;
            Publish(messageLayer, "init->start");
        }
        public void StartToTargetHandler() { Publish(messageLayer, "start->target"); }
        public void StartToEndTurnHandler() { Publish(messageLayer, "start->endturn"); }

        public void TargetToStartHandler() { Publish(messageLayer, "target->start"); }
        public void TargetToResolveHandler() { Publish(messageLayer, "resolve"); }

        public void ResolveToEndTurnHandler() { Publish(messageLayer, "resolve->endturn"); }

        public void EndTurnToExitHandler() { Publish(messageLayer, "endturn->exit"); }

        public void EndTurnToStartHandler() { Publish(messageLayer, "endturn"); }
        #endregion Handlers

        #region Triggers

        public void StartTrigger() { _fsm.ChangeState(State.START); }
        public void TargetTrigger() { _fsm.ChangeState(State.TARGET); }

        public void EndTurnTrigger() { _fsm.ChangeState(State.ENDTURN); }
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