using UnityEngine;
using System.Collections.Generic;

public class FiniteStateMachine<T>
{
    

    private List<string> _validTransistions = new List<string>();

    public List<string> TransitionTable
    {
        get { return _validTransistions; }
    }
    public T currentState
    {
        get;
        private set;
    }

    private Dictionary<string, Handler> _transitionHandlers = new Dictionary<string, Handler>();
    public void AddTransition(T from, T to, Handler h /* , Trigger t*/)
    {
        string transitionName = from.ToString().ToLower() + "->" + to.ToString().ToLower();
        _validTransistions.Add(transitionName);
        _transitionHandlers.Add(transitionName, h);
        
    }    
 
    private bool CheckTransition(T from, T to)
    {
        string transitionName = from.ToString().ToLower() + "->" + to.ToString().ToLower();        
        if (_transitionHandlers.ContainsKey(transitionName))
            return true;
      

        return false;
    }

    public void Begin(T to)
    {
        currentState = to;   
    }
    public bool ChangeState(T to)
    {
        //Debug.Log("ChangeState! " + currentState.ToString() +"->"+  to.ToString());
        if (CheckTransition(currentState, to)) //if it's in the dictionary
        {
            string transitionName = currentState.ToString().ToLower() + "->" + to.ToString().ToLower();
            _transitionHandlers[transitionName]();
            currentState = to; //set the new state

            return true;
        }

        Debug.Log("INVALID TRANSITION");
        return false;
    }
}
