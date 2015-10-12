using UnityEngine;
using System.Collections.Generic;
using Combat;

/// <summary>
/// keeps track of all units in this group and manages them
/// we will funnel all unit actions through this object before a notification
/// is registered. We do this because i don't know
/// </summary>
public class CombatParty : MonoBehaviour, IPublisher, ISubscriber
{

    void Awake()
    {
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(transform); 
    }

    bool startUpFlag;
    public void Turn(State state)
    {
 
    }

    /// <summary>
    /// move to the next unit
    /// </summary>
    private void NextUnit(int currentUnit, ref bool startup)
    {
        
        if (startup)
        {
            startup = false;
            //_currentUnit.SetState(true);
            return;
        }
        _unitIndex += 1; //increment the unit index
        turnsTaken += 1; //increment the turns taken   
        if (_unitIndex >= _partyMembers.Count)
        {            
            Publish(MessageLayer.PARTY, "finished");
            _active = false;
            //_currentUnit.SetState(false);
            _currentUnit = null;
            
            return;
        }
 
          
        _currentUnit = _partyMembers[_unitIndex];
 
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

    public void Subscribe(MessageLayer t, string e, Callback c)
    {
        EventSystem.Subscribe(t, e, c, this);
    }

    public void Subscribe<T>(MessageLayer t, string e, Callback<T> c)
    {
        EventSystem.Subscribe<T>(t, e, c, this);
    }
    #endregion Interfaces


    #region variables

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
    public CombatUnit CurrentUnit
    {
        get { return _currentUnit; }
    }
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

    #endregion variables
}
