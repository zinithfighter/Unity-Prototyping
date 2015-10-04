using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatParty : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _partyMembers;
    [SerializeField]
    private CombatUnit _currentUnit;
    [SerializeField]
    private bool active;
    private int currentUnitIndex;

    Callback onCombatStart;
    Callback onCombatIdle;

    void Awake()
    {
        if(_partyMembers.Count == 0)
        {
            foreach(Transform t in transform)
            {
                if(t.GetComponent<CombatUnit>())
                    _partyMembers.Add(t.gameObject);
            }
        }
       
        _currentUnit = _partyMembers[currentUnitIndex].GetComponent<CombatUnit>();
    }

    public void SetActive(bool s)
    {
        if (s) active = true;
        else active = false;

    }      

    void StartCombat()
    {
        _currentUnit.SetState(true);
    }
   
}
