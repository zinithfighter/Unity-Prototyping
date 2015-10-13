using UnityEngine;
using FiniteStateMachine;
using System;

public class CombatUnit : MonoBehaviour, IUnit, IPublisher, ISubscriber
{
    [SerializeField]
    int _health;
    [SerializeField]
    float _attack;
    [SerializeField]
    float _defense;
    [SerializeField]
    bool _active;

    private FiniteStateMachine<State> fsm;

    public enum State
    {
        INIT, //setup gui
        START,
        ABILITY,
        TARGET,
        RESOLVE,
        ENDTURN, //reset gui go to next party member
        EXIT,
    }

    public State currentState;

    
    void Awake()
    {
        fsm = new FiniteStateMachine<State>();
        fsm.State(State.INIT, StateHandler);
        fsm.State(State.START, StateHandler);
        fsm.State(State.ABILITY, AbilityHandler);
        fsm.State(State.TARGET, StateHandler);
        fsm.State(State.RESOLVE, ResolveHandler);
        fsm.State(State.ENDTURN, StateHandler);
        fsm.State(State.EXIT, StateHandler);

        fsm.Transition(State.INIT, State.START, "*");
        fsm.Transition(State.START, State.ABILITY, "begin");
        fsm.Transition(State.START, State.ABILITY, "space");

        fsm.Transition(State.ABILITY, State.TARGET, "attack");

        fsm.Transition(State.TARGET, State.ABILITY, "escape");

        fsm.Transition(State.TARGET, State.RESOLVE, "targetselected");

        fsm.Transition(State.ABILITY, State.RESOLVE, "endturn");
        fsm.Transition(State.ABILITY, State.RESOLVE, "defend");

        fsm.Transition(State.RESOLVE, State.ENDTURN, "space");
                
        fsm.Transition(State.ENDTURN, State.START, "space");
        fsm.Transition(State.ENDTURN, State.EXIT, "unitdone");

        Subscribe<string>(MessageLayer.INPUT, "keydown", UpdateFSM);

        Subscribe<string>(MessageLayer.GUI, "buttonclick", UpdateFSM);


        health = _health;
        attack = _attack;
        defense = _defense;
    }
    void UpdateFSM(string input)
    {
        Debug.Log("feed unit fsm with " + input);
        fsm.Feed(input);
    }

    void StateHandler()
    {
        string state = fsm.CurrentState.ToString().ToLower();
        Publish(MessageLayer.UNIT, "StateChange", state);
        Debug.Log("current state is " + state);
    }

    void ResolveHandler()
    {
        StateHandler();
    }

    void AbilityHandler()
    {
        StateHandler();
    }

    void StartHandler()
    {
        StateHandler(); 
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("idle");
    }

    private void ExitHandler()
    {
        StateHandler(); //tell everyone a unit has changed 
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("noidle");
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

    public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }
    #endregion Interfaces

    #region Variables

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
    #endregion Variables
}
