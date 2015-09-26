using UnityEngine;
using System.Collections;
using System;

public class CombatUnit : MonoBehaviour, IUnit
{    
    public int _health;
    public float _attack;
    public float _defense;

    void Awake()
    {
        _health = health;
        _attack = attack;
        _defense = defense;        
    }

    public float attack
    {
        get
        {
            return _attack;
        }

        set
        {
            _attack = value;
        }
    }

    public float defense
    {
        get
        {
            return _defense;
        }

        set
        {
            _defense = value;
        }
    }

    public int health
    {
        get
        {
            return _health;
        }

        set
        {
           _health = value;
        }
    }    
}
