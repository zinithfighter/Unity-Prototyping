using UnityEngine;
using FiniteStateMachine; 

public class CombatUnit : Observer, IUnit
{ 
    public void SetActive(bool active)
    {
        Publish(MessageLayer.UNIT, "unit change", this);
        Debug.Log("set unit " + active);
        _active = active;
        Animator anim = GetComponentInChildren<Animator>();
        if (_active)
        {
            anim.SetTrigger("idle");
        }
        else
        {
            anim.SetTrigger("noidle");
        }
        
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
 

    public float attack { get { return _attack; } set { _attack = value; } }

    public float defense { get { return _defense; } set { _defense = value; } }

    public int health { get { return _health; } set { _health = value; } }
    #endregion Variables
}
