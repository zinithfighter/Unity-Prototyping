using UnityEngine;
using FiniteStateMachine; 

public class CombatUnit : MonoBehaviour, IUnit
{

    public enum State
    {
        INIT, //setup gui
        DISABLED,
        ACTIVE,
        EXIT,
 
    }
 
    void ActiveHandler()
    {        
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("idle");
    }

    private void DisabledHandler()
    {
         //tell everyone a unit has changed 
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("noidle");
    }  

    #region Variables
    [SerializeField]
    int _health;
    [SerializeField]
    float _attack;
    [SerializeField]
    float _defense;
    [SerializeField]
    bool _active;

    private FiniteStateMachine<State> fsm;

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
    #endregion Variables
}
