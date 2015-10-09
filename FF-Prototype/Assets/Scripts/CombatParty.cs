using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// keeps track of all units in this group and manages them
/// we will funnel all unit actions through this object before a notification
/// is registered. We do this because i don't know
/// </summary>
public class CombatParty : MonoBehaviour, IPublisher, ISubscriber
{
    /// <summary>
    /// the list of units participating in combat
    /// </summary>
    [SerializeField]
    private List<CombatUnit> _partyMembers;

    /// <summary>
    /// the instance of the current unit taking turn
    /// </summary>
    [SerializeField]
    private CombatUnit _currentUnit;
    /// <summary>
    /// current index of the unit taking turn
    /// </summary>
    [SerializeField]
    private int _unitIndex;

    [SerializeField]
    private bool _active;

    /// <summary>
    /// how many turns we have taken
    /// </summary>
    public int turnsTaken;

    /// <summary>
    /// number of members in the group
    /// </summary>    
    public int PartySize
    {
        get { return _partyMembers.Count; }
    }

    void Awake()
    {
        _active = false;        
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(this.transform);
        _currentUnit = _partyMembers[_unitIndex];
        Subscribe(MessageType.COMBAT, "resolve", NextUnit);
    }

    /// <summary>
    /// Setting the state will cause this party to become active
    /// </summary>
    /// <param name="state"></param>
    public void SetState(bool state)
    {
        _active = state;
        if (_active)
        { 
            NextUnit();
        }
        else if (!_active)
        {
            _unitIndex = -1;        
            turnsTaken = 0;
        }
    } 

    /// <summary>
    /// move to the next unit
    /// </summary>
    void NextUnit()
    {
        _currentUnit.SetState(false);
        _unitIndex += 1; //increment the unit index
        turnsTaken += 1; //increment the turns taken

        if (_unitIndex >= _partyMembers.Count)
        {
            Publish(MessageType.PARTY, "finished");
            
        }
        else
        {
            _currentUnit = _partyMembers[_unitIndex];
            _currentUnit.SetState(true);
        }


        

        Publish(MessageType.COMBAT, "unit change", _currentUnit); //tell everyone a unit has shifted 

        

    }

    #region Interfaces
    public void Publish(MessageType m, string e)
    {
        EventSystem.Broadcast(m, e);
    }

    public void Publish<T>(MessageType m, string e, T args)
    {
        EventSystem.Broadcast<T>(m, e, args);
    }

    public void Subscribe(MessageType t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }

    public void Subscribe<T>(MessageType t, string e, Callback<T> c)
    {
        EventSystem.Subscribe<T>(t, e, c, this);
    }
    #endregion Interfaces
}
