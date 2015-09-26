using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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

    public class CombatSystem : MonoBehaviour, IPublisher
    {        
        public List<GameObject> CombatParties;
        private FiniteStateMachine<State> _fsm;
        public List<string> trans;     

        void Awake()
        {
            _fsm = new FiniteStateMachine<State>();
            _fsm.AddTransition(State.INIT, State.START, InitToStart);
            _fsm.AddTransition(State.START, State.TARGET, StartToTarget);
            _fsm.AddTransition(State.TARGET, State.RESOLVE, TargetToResolve);
            _fsm.AddTransition(State.RESOLVE, State.ENDTURN, ResolveToEndTurn);
            _fsm.AddTransition(State.ENDTURN, State.ENDTURN, EndTurnToExit);
            _fsm.AddTransition(State.ENDTURN, State.START, EndTurnToStart);

            _fsm.Begin(State.INIT);

            trans.AddRange(_fsm.TransitionTable);
        }

        void Start()
        {
            ChangeState(State.START);
        }

        public void Publish(string e)
        {
            string type = "combat";
            string message = type + ":" + e;
            EventSystem.Notify(message);
        }

        public void ChangeState(State s)
        {
            _fsm.ChangeState(s);
        }

        public void InitToStart()
        {
            Publish("init->start");
        }

        public void StartToTarget()
        { 
            Publish("start->target");
        }

        public void TargetToResolve()
        {
            Publish("target->resolve");
        }

        public void ResolveToEndTurn()
        {
            Publish("resolve->endturn");
        }

        public void EndTurnToExit()
        {
            Publish("endturn->exit");
        }

        public void EndTurnToStart()
        {
            Publish("endturn->start");
        }
    }
}