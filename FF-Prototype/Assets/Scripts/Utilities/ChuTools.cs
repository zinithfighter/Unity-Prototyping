using UnityEngine; 
using System.Collections.Generic;

static public class ChuTools
{
    //partyMembers = ChuTools.PopulateFromChildren(this, CombatUnit, partyMembers);
    static public List<C> PopulateFromChildren<C>(Transform transform)
    {
        List<C> children = new List<C>();
        foreach (Transform t in transform)
        {
            if(t.GetComponent<C>() != null)
                children.Add(t.GetComponent<C>());
        }

        return children; 
    }
}