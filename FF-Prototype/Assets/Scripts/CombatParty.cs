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
    [SerializeField]
    private List<CombatUnit> _partyMembers;
    [SerializeField]
    private CombatUnit _currentUnit;
    [SerializeField]
    private int _unitIndex;

    void Awake()
    {
        _unitIndex = 0;
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(this.transform);
        _currentUnit = _partyMembers[_unitIndex];
    }

    void Start()
    {
        EventSystem.Broadcast(MessageType.COMBAT, "unit change", _currentUnit);
    }


    public void NextUnit()
    {
        _currentUnit.SetState(false);

        _unitIndex += 1;

        if (_unitIndex >= _partyMembers.Count)
            _unitIndex = 0;

        _currentUnit = _partyMembers[_unitIndex];

        _currentUnit.SetState(true);
        
        EventSystem.Broadcast(MessageType.COMBAT, "unit change", _currentUnit);
    }

    public void Publish(MessageType m, string e)
    {
        EventSystem.Broadcast(m, e);
    }
}
