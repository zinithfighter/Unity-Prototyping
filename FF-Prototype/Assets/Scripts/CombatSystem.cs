using UnityEngine;
using System;
using System.Collections;
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

        void Awake()
        {
            _fsm = new FiniteStateMachine<State>();
            _fsm.AddTransition(State.INIT, State.START, InitToStartHandler);
            _fsm.AddTransition(State.START, State.TARGET, StartToTargetHandler);
            _fsm.AddTransition(State.TARGET, State.START, TargetToStartHandler);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, TargetToResolveHandler);
            _fsm.AddTransition(State.RESOLVE, State.ENDTURN, ResolveToEndTurnHandler);
            _fsm.AddTransition(State.ENDTURN, State.EXIT, EndTurnToExitHandler);
            _fsm.AddTransition(State.ENDTURN, State.START, EndTurnToStartHandler);

            _fsm.Begin(State.INIT);

            Subscribe(MessageType.GUI, "attack", StartToTargetTrigger);
            Subscribe(MessageType.GUI, "cancel", TargetToStartTrigger);
        }

        void Start()
        {
            _fsm.ChangeState(State.START);
        } 
        
        #region Handlers
        public void InitToStartHandler()
        {
            Publish(messageLayer,"init->start");
        }

        public void StartToTargetHandler()
        { 
            Publish(messageLayer,"start->target");
            
        }
        
        public void TargetToStartHandler()
        {
            Publish(messageLayer, "target->start");
        }
        
        public void TargetToResolveHandler()
        {
            Publish(messageLayer, "target->resolve");
        }

        public void ResolveToEndTurnHandler()
        {
            Publish(messageLayer, "resolve->endturn");
        }

        public void EndTurnToExitHandler()
        {
            Publish(messageLayer, "endturn->exit");
        }

        public void EndTurnToStartHandler()
        {
            Publish(messageLayer, "endturn->start");
        }
        #endregion Handlers

        #region Triggers
        public void StartToTargetTrigger()
        {
            _fsm.ChangeState(State.TARGET);
        }
        public void TargetToStartTrigger()
        {
            _fsm.ChangeState(State.START);
        }
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
        #endregion Interface
    }
}