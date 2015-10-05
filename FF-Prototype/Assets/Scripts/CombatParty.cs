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

    public bool active;

    void Awake()
    {
        _unitIndex = 0;
        _partyMembers = ChuTools.PopulateFromChildren<CombatUnit>(this.transform);
        _currentUnit = _partyMembers[_unitIndex];
        Subscribe(MessageType.COMBAT, "endturn", NextUnit);
        Subscribe(MessageType.COMBAT, "init->start", Start);
    }

 
    void Start()
    {
        if (active)
        {
            _currentUnit.SetState(true);
            Publish(MessageType.COMBAT, "unit change", _currentUnit);
        }
    }


    public void NextUnit()
    {
        if (active)
        {
            _currentUnit.SetState(false);

            _unitIndex += 1;

            if (_unitIndex >= _partyMembers.Count)
                _unitIndex = 0;

            _currentUnit = _partyMembers[_unitIndex];

            _currentUnit.SetState(true);

            Publish(MessageType.COMBAT, "unit change", _currentUnit);
        }
    }

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
}
