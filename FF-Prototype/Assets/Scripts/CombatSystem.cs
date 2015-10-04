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
            _fsm.AddTransition(State.INIT, State.START, InitToStart);
            _fsm.AddTransition(State.START, State.TARGET, StartToTarget);
            _fsm.AddTransition(State.TARGET, State.START, TargetToStart);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, TargetToResolve);
            _fsm.AddTransition(State.RESOLVE, State.ENDTURN, ResolveToEndTurn);
            _fsm.AddTransition(State.ENDTURN, State.EXIT, EndTurnToExit);
            _fsm.AddTransition(State.ENDTURN, State.START, EndTurnToStart);

            _fsm.Begin(State.INIT);

            Subscribe(MessageType.GUI, "attack", StartToTarget); 
        }

        void Start()
        {
            ChangeState(State.START);
        } 

        public void ChangeState(State s)
        {
            _fsm.ChangeState(s);
        }

        public void InitToStart()
        {
            Publish(messageLayer,"init->start");
        }

        public void StartToTarget()
        { 
            Publish(messageLayer,"start->target");
        }

        public void TargetToStart()
        {
            Publish(messageLayer, "target->start");
        }

        public void TargetToResolve()
        {
            Publish(messageLayer, "target->resolve");
        }

        public void ResolveToEndTurn()
        {
            Publish(messageLayer, "resolve->endturn");
        }

        public void EndTurnToExit()
        {
            Publish(messageLayer, "endturn->exit");
        }

        public void EndTurnToStart()
        {
            Publish(messageLayer, "endturn->start");
        }

        public void Publish(MessageType m, string e)
        {
            EventSystem.Broadcast(m, e);
        }

        public void Subscribe(MessageType t, string e, Callback c)
        {
           EventSystem.Subscribe(t, e, c, this);
        }
    }
}