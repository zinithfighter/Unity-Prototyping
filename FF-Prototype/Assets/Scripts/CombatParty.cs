using UnityEngine;
using System.Collections.Generic;
using Unit;

namespace Party
{ /// <summary>
  /// keeps track of all units in this group and manages them
  /// we will funnel all unit actions through this object before a notification
  /// is registered. We do this because i don't know
  /// </summary>

    public class CombatParty : MonoBehaviour
    {
        void Awake()
        {

        }

        void NextUnit()
        {

        }

        #region Variables        
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
        public string partyName
        {
            get { return name; }
            private set { partyName = name; }
        }
        /// <summary>
        /// number of members in the group
        /// </summary>    
        public int partySize
        {
            get { return _partyMembers.Count; }
        }

        #endregion Variables
    }
}