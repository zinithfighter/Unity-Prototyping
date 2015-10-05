using UnityEngine;
using System.Collections;
using System;

public class CombatUnit : MonoBehaviour, IUnit
{
    [SerializeField]
    int _health;
    [SerializeField]
    float _attack;
    [SerializeField]
    float _defense;    
    bool active;

    FiniteStateMachine<State> _fsm;

    public enum State
    {
        init,
        active,
        inactive,
        exit,

    }

    public State currentState = State.init;

    public void SetState(bool s)
    {
        active = s;
        if (active) _fsm.ChangeState(State.active);
        else _fsm.ChangeState(State.inactive);

    }

    void Awake()
    {
        _fsm = new FiniteStateMachine<State>();
        _fsm.AddTransition(State.init, State.inactive, InitInactive);
        _fsm.AddTransition(State.active, State.inactive, ActiveInactive);
        _fsm.AddTransition(State.inactive, State.active, InactiveActive);
        _fsm.AddTransition(State.inactive, State.exit, InactiveExit);

        health = _health;
        attack = _attack;
        defense = _defense;

        _fsm.Begin(State.init);

        SetState(false);

    }

    void Update()
    {
        currentState = _fsm.currentState;
    }
    private void StartUp()
    {
        _fsm.ChangeState(State.active);
    }

    private void ShutDown()
    {
        _fsm.ChangeState(State.inactive);
    }

    private void InitInactive()
    {

    }

    private void ActiveInactive()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("noidle");
    }

    private void InactiveActive()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("idle");
    }

    private void InactiveExit()
    {

    }


    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
}
