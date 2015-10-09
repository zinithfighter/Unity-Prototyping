using System;
using System.Collections.Generic;

public class FiniteStateMachine<T>
{
    private List<T> _states = new List<T>();
    private List<string> _validTransistions = new List<string>();
    private Dictionary<string, Delegate> _transitionHandlers = new Dictionary<string, Delegate>();

    public FiniteStateMachine()
    {
        Type enumType = typeof(T);
        var states = (T[])Enum.GetValues(enumType);
        foreach(var state in states)
        {
            _states.Add(state);
        }

        currentState = _states[0];
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

    
    public void AddTransition(T from, T to, Handler h /* , Trigger t*/)
    {
        string transitionName = from.ToString().ToLower() + "->" + to.ToString().ToLower();
        _validTransistions.Add(transitionName);
        _transitionHandlers.Add(transitionName, h);

    }
 
    public bool ChangeState(T to)
    {
        string transitionName = currentState.ToString().ToLower() + "->" + to.ToString().ToLower();
        if (_transitionHandlers.ContainsKey(transitionName))
        { 
            currentState = to; //set the new state
            //all delegates are called when the machine
            //enters a *NEW* state
            Handler handler = (Handler)_transitionHandlers[transitionName];
            handler();


            return true;
        }   

        //Debug.Log("INVALID TRANSITION! " + transitionName);
        return false;
    }
}
