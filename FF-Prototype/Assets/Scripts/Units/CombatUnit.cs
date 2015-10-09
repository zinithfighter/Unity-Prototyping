using UnityEngine;
using System.Collections;
using System;

public class CombatUnit : MonoBehaviour, IUnit, IPublisher
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
        _fsm.AddTransition(State.INIT, State.INACTIVE, EnterInactiveHandler);
        _fsm.AddTransition(State.INACTIVE, State.ACTIVE, EnterActiveHandler);
        _fsm.AddTransition(State.ACTIVE, State.INACTIVE, EnterInactiveHandler);
        _fsm.AddTransition(State.INACTIVE, State.EXIT, null);

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
        if (state)
        {
            _fsm.ChangeState(State.ACTIVE);
        }
        else
        {
            _fsm.ChangeState(State.INACTIVE);
        }
    } 

    private void EnterInactiveHandler()
    {
        _active = false;
        Animator anim = GetComponentInChildren<Animator>();
        Debug.Log("set idle trigger inactive");
        anim.SetTrigger("noidle");
    }

    private void EnterActiveHandler()
    {
        _active = true;
        Publish(MessageType.COMBAT, "unit change", this); //tell everyone a unit has shifted 
        Animator anim = GetComponentInChildren<Animator>();
        Debug.Log("set idle trigger active");
        anim.SetTrigger("idle");
    }

    public void Publish(MessageType m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Publish<T>(MessageType m, string e, T args)
    {
        EventSystem.Broadcast<T>(m, e, args);
    }

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
}
