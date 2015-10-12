using System; 

public interface IFiniteStateMachine
{
    bool AddState(Enum state, Delegate Handler);
    bool AddTransition(Enum stateA, Enum stateB, string input);
}
