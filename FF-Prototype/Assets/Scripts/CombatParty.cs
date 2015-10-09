using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// keeps track of all units in this group and manages them
/// we will funnel all unit actions through this object before a notification
/// is registered. We do this because i don't know
/// </summary>
public class CombatParty : MonoBehaviour, IPublisher
{
    /// <summary>
    /// the list of units participating in combat
    /// </summary>
    [SerializeField]
    private List<CombatUnit> _partyMembers;

    /// <summary>
    /// current index of the unit taking turn
    /// </summary>
    [SerializeField]
    private int _unitIndex;

    [SerializeField]
    private bool _active;

    /// <summary>
    /// the instance of the current unit taking turn
    /// </summary>
    [SerializeField]
    private CombatUnit _currentUnit;
    /// <summary>
    /// how many turns we have taken
    /// </summary>
    public int turnsTaken;

    /// <summary>
    /// number of members in the group
    /// </summary>    
    public int partySize
    {
        get { return _partyMembers.Count; }
    }

    void Awake()
    {
        _active = false;
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(this.transform);
        _currentUnit = _partyMembers[_unitIndex];
    }

    public bool active
    {
        get { return _active; }
        set { _active = value; }
    }

    public void StartParty()
    {
        NextUnit(-1);
        
        
    }

    public void Next()
    {
        NextUnit(_unitIndex);
    }
    /// <summary>
    /// move to the next unit
    /// </summary>
    private void NextUnit(int unit)
    {
        _currentUnit.SetState(false); 

        if (unit >= _partyMembers.Count)
        {
            Publish(MessageType.PARTY, "finished");
        }
        else if(unit < 0)
        {
            _unitIndex += 1; //increment the unit index
            turnsTaken += 1; //increment the turns taken
        }
        else
        {
            _currentUnit = _partyMembers[_unitIndex];
            _currentUnit.SetState(true);
        }

         



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
    #endregion Interfaces
}
