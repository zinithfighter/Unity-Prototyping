using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatParty : MonoBehaviour {

    public List<GameObject> _partyMembers;
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
    }

   
}
