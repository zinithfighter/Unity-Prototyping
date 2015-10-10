using System;
using System.Collections.Generic;

public class FiniteStateMachine<T>
{

    class TransitionHandler
    {
        public Handler enter;
        public Handler exit;
        public TransitionHandler(Handler pre, Handler post)
        {
            enter = pre;
            exit = post;
        }
    }

    private List<T> _states = new List<T>();
    private List<string> _validTransistions = new List<string>();
    private Dictionary<string, TransitionHandler> _transitions = new Dictionary<string, TransitionHandler>();


    public FiniteStateMachine()
    {
        Type enumType = typeof(T);
        var states = (T[])Enum.GetValues(enumType);
        foreach (var state in states)
        {
            _states.Add(state);
        }

        currentState = _states[0];
        previousState = _states[0];
    }

    public List<string> TransitionTable
    {
        get { return _validTransistions; }
    }

    public List<T> States
    {
        get { return _states; }
    }
    public T currentState
    {
        get;
        private set;
    }

    public T previousState
    { 
        get;
        private set;
    }

    public void AddTransition(T from, T to, Handler enter)
    {
        string transitionName = from.ToString().ToLower() + "->" + to.ToString().ToLower();
        _validTransistions.Add(transitionName);
        TransitionHandler t = new TransitionHandler(enter, null);
        _transitions.Add(transitionName, t);
    }

    public void AddTransition(T from, T to, Handler enter, Handler exit)
    {
        string transitionName = from.ToString().ToLower() + "->" + to.ToString().ToLower();
        _validTransistions.Add(transitionName);
        TransitionHandler t = new TransitionHandler(enter, exit);
        _transitions.Add(transitionName, t);

    }

    public bool ChangeState(T to)
    {
        string transitionName = currentState.ToString().ToLower() + "->" + to.ToString().ToLower();
        if (_transitions.ContainsKey(transitionName))
        {
            if (_transitions[transitionName].exit != null)
                _transitions[transitionName].exit();

            previousState = currentState;
            currentState = to; //set the new state  

            if (_transitions[transitionName].enter != null)
                _transitions[transitionName].enter();

              

            
            return true;
        }

        //Debug.Log("INVALID TRANSITION! " + transitionName);
        return false;
    }

    public bool ReverseState()
    {
        string transitionName = previousState.ToString().ToLower() + "->" + currentState.ToString().ToLower();
        if (_transitions.ContainsKey(transitionName))
        { 
            if (_transitions[transitionName].enter != null)
                _transitions[transitionName].enter();
            currentState = previousState; //set the new state        
            previousState = currentState;

            if (_transitions[transitionName].exit != null)
                _transitions[transitionName].exit();
            return true;
        }

        //Debug.Log("INVALID TRANSITION! " + transitionName);
        return false;
    }
}
