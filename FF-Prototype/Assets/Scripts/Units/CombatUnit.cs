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
    [SerializeField]
    bool _active;

    private FiniteStateMachine<State> _fsm;

    public enum State
    {
        INIT,
        INACTIVE,
        ACTIVE,
        EXIT,
    }

    public State currentState;

    void Awake()
    {
        _fsm = new FiniteStateMachine<State>();
        _fsm.AddTransition(State.INIT, State.INACTIVE, EnterInactive);
        _fsm.AddTransition(State.INACTIVE, State.ACTIVE, EnterActive);
        _fsm.AddTransition(State.ACTIVE, State.INACTIVE, EnterInactive);
        _fsm.AddTransition(State.INACTIVE, State.EXIT, EnterExit);

        health = _health;
        attack = _attack;
        defense = _defense;
        SetState(false);
    }
 
    void Update()
    {
        currentState = _fsm.currentState;
    }

    public void SetState(bool state)
    {
        ///(input_parameters) => {statement;}
        //Callback doit = (state) ? (Callback)StartUp : (Callback)ShutDown;
        //doit();

        if (state)
            StartUp();
        else
            ShutDown();
    }

    private void StartUp()
    {
        _active = true;
        _fsm.ChangeState(State.ACTIVE);
    }

    private void ShutDown()
    {
        _active = false;
        _fsm.ChangeState(State.INACTIVE);
    }

    private void EnterInactive()
    {
        Animator anim = GetComponentInChildren<Animator>();
        Debug.Log("set idle trigger inactive");
        anim.SetTrigger("noidle");
    }

    private void EnterActive()
    {
        Animator anim = GetComponentInChildren<Animator>();
        Debug.Log("set idle trigger active");
        anim.SetTrigger("idle");
    }

    private void EnterExit()
    {

    }

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
}
