using UnityEngine;
using System.Collections.Generic;
using FiniteStateMachine;
using Party;

namespace Combat
{
    enum State
    {
        INIT,
        START,
        ACTIVE,
        PAUSED,
        TARGET,     
        ENDTURN,   
        RESOLVE, //Determine if this party needs to be changed out
        EXIT,
    }

    //responsible for intercepting gui input and relaying it
    public class CombatSystem : Observer
    {
        void Awake()
        {
            fsm = new FiniteStateMachine<State>();
            fsm.State(State.INIT, InitHandler);
            fsm.State(State.START, StartHandler);
            fsm.State(State.ACTIVE, ActiveHandler);
            fsm.State(State.TARGET, TargetHandler);
            fsm.State(State.RESOLVE, ResolveHandler);
            fsm.State(State.ENDTURN, EndTurnHandler);
            fsm.State(State.EXIT, ExitHandler);

            fsm.Transition(State.INIT, State.START, "start");//combat

            fsm.Transition(State.START, State.ACTIVE, "space");//input

            fsm.Transition(State.ACTIVE, State.ENDTURN, "endturn");//gui
            fsm.Transition(State.ACTIVE, State.ENDTURN, "space");//gui

            fsm.Transition(State.ENDTURN, State.RESOLVE, "turn");//party

            fsm.Transition(State.RESOLVE, State.ACTIVE, "space");//input
            fsm.Transition(State.RESOLVE, State.START, "exit");//party
            fsm.Transition(State.RESOLVE, State.EXIT, "quit");//party

            UpdateFSM("*");
        }

        void Start()
        {
            UpdateFSM("start");
        }        

        void UpdateFSM(string input)
        {
           // Debug.Log("feed combat fsm with " + input);
            fsm.Feed(input);
        }

        void OnStateChange(State state)
        {
            Debug.Log("State change: Combat: " + state.ToString().ToLower());
            Publish(MessageLayer.COMBAT, "enter state", state.ToString().ToLower());
        }

        void InitHandler()
        {
            //input events that will drive this fsm
            //"space" "escape"
            Subscribe<string>(MessageLayer.INPUT, "key down", UpdateFSM);
            //gui events that will drive this fsm
            //"attack" "end turn" "defend"
            Subscribe<string>(MessageLayer.GUI, "button click", UpdateFSM);

            //trigger resolve to active for this machine
            Subscribe<string>(MessageLayer.PARTY, "state change", UpdateFSM);
            Subscribe<string>(MessageLayer.GAME, "state change", UpdateFSM);
            _combatParties = ChuTools.PopulateFromChildren<CombatParty>(transform);
            _partyIndex = 0;
        }

        void StartHandler()
        {
            OnStateChange(State.START);            
            currentParty = _combatParties[_partyIndex];
            Publish(MessageLayer.COMBAT, "party change", currentParty);    
        }

        void ActiveHandler()
        {            
            OnStateChange(State.ACTIVE);
            currentParty.Activate();
        }

        void EndTurnHandler()
        {
            OnStateChange(State.ENDTURN);
        }

        void ResolveHandler()
        {
            OnStateChange(State.RESOLVE);
            if (_partyIndex >= _combatParties.Count - 1)
            {
                _partyIndex = 0;
                return;
            }  
            
            if(currentParty.turnsTaken >= currentParty.partySize -1)
            {
                _partyIndex++;
            }
            
        }
        
        void TargetHandler()
        {
            OnStateChange(State.TARGET);
        }

        void ExitHandler()
        {
            OnStateChange(State.EXIT);
        }        

        #region Variables


        private FiniteStateMachine<State> fsm;

        [SerializeField]
        private List<CombatParty> _combatParties;

        [SerializeField]
        private CombatParty currentParty;

         

        int _partyIndex;

        #endregion Variables
    }


}