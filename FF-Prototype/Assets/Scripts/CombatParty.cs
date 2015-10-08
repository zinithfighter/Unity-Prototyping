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
    [SerializeField]
    private List<CombatUnit> _partyMembers;
    [SerializeField]
    private CombatUnit _currentUnit;
    [SerializeField]
    private int _unitIndex;

    [SerializeField]
    private bool active;

    public int turnsTaken;

    private int _partySize;
    public int PartySize { get { return _partyMembers.Count; } }
    void Awake()
    {
        _unitIndex = 0;
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(this.transform);
        _currentUnit = _partyMembers[_unitIndex];
        Subscribe(MessageType.COMBAT, "init->start", StartUp);
        Subscribe(MessageType.COMBAT, "endturn->start", NextUnit);
        Subscribe(MessageType.COMBAT, "endturn->exit", ShutDown);
    }

    public void SetState(bool state)
    {
        active = state;
    }

    void StartUp()
    {
        turnsTaken = 0;
        NextUnit();
    }

    void ShutDown()
    {
        _unitIndex = 0;
        active = false;
    }

    void NextUnit()
    {
        bool rollOver = false;
        if (active)
        {
            _currentUnit.SetState(false);

            _unitIndex += 1;
            turnsTaken += 1;

            if (_unitIndex >= _partyMembers.Count)
            {
                if (rollOver) _unitIndex = 0;
                else
                {
                    ShutDown();
                    Publish(MessageType.PARTY, "Finished");

                    return;
                }
            }

            _currentUnit = _partyMembers[_unitIndex];

            _currentUnit.SetState(true);

            Publish(MessageType.COMBAT, "unit change", _currentUnit);
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
