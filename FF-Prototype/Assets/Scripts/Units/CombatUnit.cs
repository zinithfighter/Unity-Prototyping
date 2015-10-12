using UnityEngine;
using FiniteStateMachine;
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


        health = _health;
        attack = _attack;
        defense = _defense;
    }
 
    void Update()
    {
         
    }

    private void EnterInactiveHandler()
    {
        _active = false;
        Animator anim = GetComponentInChildren<Animator>();
        //Debug.Log("set idle trigger inactive");
        anim.SetTrigger("noidle");
    }

    private void EnterActiveHandler()
    {
        _active = true;
        Publish(MessageLayer.UNIT, "unit change", this); //tell everyone a unit has changed 
        Animator anim = GetComponentInChildren<Animator>();
        //Debug.Log("set idle trigger active");
        anim.SetTrigger("idle");
    }

    public void Publish(MessageLayer m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Publish<T>(MessageLayer m, string e, T args)
    {
        EventSystem.Broadcast<T>(m, e, args);
    }

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
}
