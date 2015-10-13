using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;

/// <summary>
/// keeps track of all units in this group and manages them
/// we will funnel all unit actions through this object before a notification
/// is registered. We do this because i don't know
/// </summary>
public class CombatParty : MonoBehaviour, IPublisher, ISubscriber
{
    enum State
    {
        INIT,
        START,
        ACTIVE,        
        RESOLVE,
        EXIT,
    }

    FiniteStateMachine<State> fsm;
    void StateChange()
    {
        string currentState = fsm.CurrentState.ToString();
        Publish(MessageLayer.PARTY, currentState);
    }
    void ResolveHandler()
    {
        StateChange();
        if (_unitIndex >= _partyMembers.Count)        
           fsm.Feed("partydone");        
        else
            fsm.Feed("next");
    }

    void ActiveHandler()
    {
        StateChange();
        if (_unitIndex >= _partyMembers.Count)
            _currentUnit = _partyMembers[0];
        else
        {
            _unitIndex += 1; //increment the unit index
            turnsTaken += 1; //increment the turns taken 
            _currentUnit = _partyMembers[_unitIndex];
        }
    }

    void StartHandler()
    {
        StateChange();
        _unitIndex = 0;
    }

    void ExitHandler()
    {
        StateChange();
        _unitIndex = 0;
        //destroy this party
    }
    void Awake()
    {
        fsm = new FiniteStateMachine<State>();
        fsm.State(State.INIT, StateChange);
        fsm.State(State.RESOLVE, ResolveHandler);
        fsm.State(State.START, StartHandler);
        fsm.State(State.ACTIVE, StateChange);
        fsm.State(State.EXIT, StateChange);

        fsm.Transition(State.INIT, State.START, "*");
        fsm.Transition(State.START, State.ACTIVE, "start");
        fsm.Transition(State.ACTIVE, State.RESOLVE, "resolve");
        fsm.Transition(State.RESOLVE, State.ACTIVE, "next");

        fsm.Transition(State.RESOLVE, State.EXIT, "dead");        
        fsm.Transition(State.RESOLVE, State.START, "partydone");

        Subscribe<string>(MessageLayer.COMBAT, "start", UpdateFSM);

        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(transform);
        
    }
 
    void Start()
    {
        UpdateFSM("*");
    }
    
    void UpdateFSM(string input)
    {
        Debug.Log("feed party fsm with " + input);
        fsm.Feed(input);
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
