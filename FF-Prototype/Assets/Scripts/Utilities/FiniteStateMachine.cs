using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// States need to be well defined by their name as well as their valid inputs
/// </summary>
namespace FiniteStateMachine
{
    /// <summary>
    /// State object to represent a current state in the machine
    /// </summary>
    public class State
    {
        /// <summary>
        /// the callback function to execute when the state becomes active
        /// </summary>
        public Delegate handler;

        /// <summary>
        /// unique identifier for this state
        /// </summary>
        public int id;

        /// <summary>
        /// the name of this state
        /// </summary>
        public Enum name;

        /// <summary>
        /// Create a state object
        /// </summary>
        /// <param name="stateName">
        /// name of the state used as key into dictionary
        /// </param>
        /// <param name="stateHandler">
        /// delegate to be executed when this state becomes active
        /// </param>
        public State(Enum stateName, Delegate stateHandler)
        {
            name = stateName;
            handler = stateHandler;
            id = GetHashCode();
        }

        /// <summary>
        /// public method to execute the delegate attached to this state
        /// </summary>
        /// <returns>
        /// false if something...
        /// </returns>
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
    /// Transition object to represent valid transitions for a given state
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// unique identifier for this transition
        /// </summary>
        private int id;

        /// <summary>
        /// defines the transition
        /// </summary>
        public string input
        {
            get;
            private set;
        }

        /// <summary>
        /// where to transition to
        /// </summary>
        public State destination
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a transition object to attach to a state
        /// </summary>
        /// <param name="token">
        /// the required input to change to the destination
        /// </param>
        /// <param name="to">
        /// state to transition to given token
        /// </param>
        public Transition(string token, State to)
        {
            input = token;
            destination = to;
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
        /// <summary>
        /// create a state machine to manage states
        /// </summary>
        public FiniteStateMachine()
        {
            states = new List<State>();
            table = new Dictionary<Enum, List<Transition>>();
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
                table.Add(state.name, new List<Transition>());
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
        public bool Feed(string token)
        {
            if (token == "*")
            {
                currentState.Handler();
                return true;
            } 

            foreach (Transition t in table[currentState.name])
            {
                if (t.input == token)
                {
                    currentState = t.destination;
                    currentState.Handler();
                    return true;
                }
            }

            return false;
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
            State newState = states.Find(x => x.name == state);
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
            Enum from = stateA as Enum;
            Enum to = stateB as Enum;
            State destination = states.Find(state => state.name == to);
            if (table.ContainsKey(from))
            {
                Transition transition = new Transition(input.ToString(), destination);
                table[from].Add(transition);
            }
            else
            {
                throw (new NotImplementedException("state does not exist"));

            }

            return true;
        }

        /// <summary>
        /// return the current state of the fsm
        /// </summary>
        private State currentState
        {
            get;
            set;
        }

        #region Variables
        List<State> states;

        Dictionary<Enum, List<Transition>> table;

        #endregion Variables

    }
}
