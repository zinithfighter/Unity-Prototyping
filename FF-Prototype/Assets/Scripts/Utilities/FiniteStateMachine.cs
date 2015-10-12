using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// States need to be well defined by their name as well as their valid inputs
/// </summary>
namespace FiniteStateMachine
{
    public class State
    {
        public Delegate handler;
        public string id;
        public Enum name;


        public State(Enum stateName, Delegate stateHandler)
        {
            name = stateName;
            handler = stateHandler;
            id = stateName.ToString();
        }

        public bool Handler()
        {
            Handler h;
            if (handler != null)
            {
                h = handler as Handler;
                h();
                return true;
            }
            return false;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Transition
    {
        int id;
        public string input
        {
            get;
            private set;
        }
        public State destination
        {
            get;
            private set;
        }
        public Transition(string token, State state)
        {
            input = token;
            destination = state;
            id = GetHashCode();
        }
    }

    /// <summary>
    /// finite state machine with 
    /// </summary>
    /// <typeparam name="T">
    /// expecting an enum but can take in any kind of type</typeparam>
    public class FiniteStateMachine<T>
    {
        List<State> states; 
        Dictionary<string, List<Transition>> table;
        public FiniteStateMachine()
        {
            states = new List<State>(); 
            table = new Dictionary<string, List<Transition>>();
            Type stateType = typeof(T);
            //if enum
            var values = (T[])Enum.GetValues(stateType);
            if (states == null)
                states = new List<State>();
            foreach (var v in values)
            {
                Enum enumType = v as Enum;
                State state = new State(enumType, null);
                states.Add(state);
                table.Add(state.id, new List<Transition>());
            }

            currentState = states[0];
        }



        /// <summary>
        /// give input to the machine. if the machine returns true for the given state then transition
        /// </summary>
        /// <typeparam name="V">Can be any kind of input that is associated with the current state
        /// for example string or integer values </typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public State Feed<V>(V token)
        {
            foreach (Transition t in table[currentState.id])
            {
                if (t.input == token.ToString())
                {                    
                    currentState = t.destination;
                    currentState.Handler();            
                }
            }

            return currentState;
        }

        /// <summary>
        /// Set a states behaviour
        /// </summary>
        /// <param name="stateA">
        /// state to set behaviour for</param>
        /// <param name="handler">
        /// what will execute when this becomes the current state</param>
        /// <returns></returns>
        public bool State(T stateA, Handler handler)
        {
            Enum state = stateA as Enum;
            State newState = states.Find(x => x.id == stateA.ToString());
            newState.handler = handler;
            
            
            return true;
        }

 
        /// <summary>
        /// Create transition between two states
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="stateA"></param>
        /// <param name="stateB"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Transition<V>(T stateA, T stateB, V input)
        {
            State destination = states.Find(state => state.id == stateB.ToString());            
            if (table.ContainsKey(stateA.ToString()))
            {
                Transition transition = new Transition(input.ToString(), destination);
                table[stateA.ToString()].Add(transition);
            }
            else
            {
                throw (new NotImplementedException("state does not exist"));

            }

            return true;
        }
 

        /// <summary>
        /// return current state of the fsm
        /// </summary>
        public Enum CurrentState
        {
            get
            {
                return currentState.name as Enum;
            }
        }

        /// <summary>
        /// return the current state of the fsm
        /// </summary>
        private State currentState
        {
            get;
            set;
        }


    }
}
