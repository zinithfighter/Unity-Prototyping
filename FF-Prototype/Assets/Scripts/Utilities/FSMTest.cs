using UnityEngine;
using FiniteStateMachine;

public class FSMTest : MonoBehaviour
{
    enum State
    {
        init,
        idle,
        walk,
        run
    }
    FiniteStateMachine<State> fsm;
    // Use this for initialization
    void Awake()
    {
        fsm = new FiniteStateMachine<State>();
        fsm.State(State.init, InitHandler);
        fsm.State(State.idle, IdleHandler);
        fsm.State(State.walk, WalkHandler);
        fsm.State(State.run, RunHandler);

        fsm.Transition(State.init, State.idle, "*");
        fsm.Transition(State.idle, State.idle, "@");
        fsm.Transition(State.idle, State.walk, "w-down");
        fsm.Transition(State.walk, State.idle, "w-up");
        fsm.Transition(State.walk, State.run, "shift-down");
        fsm.Transition(State.run, State.walk, "shift-up");
    }
    void f(string s)
    {
        fsm.Feed(s);
    }
    string fsmfeed = "*";
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            fsmfeed = "w-down";
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            fsmfeed = "w-up";
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            fsmfeed = "shift-down";
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            fsmfeed = "shift-up";
        }

        f(fsmfeed);
    }

    void InitHandler()
    {
        Debug.Log("perform init handler operations");
    }

    void IdleHandler()
    {
        idleTimer += Time.deltaTime;
        Debug.Log("IDLE: " + idleTimer);
    }
    float walkTimer = 0;
    float idleTimer = 0;
    float runTimer = 0;
    void WalkHandler()
    {
        walkTimer += Time.deltaTime;
        Debug.Log("Walking: " + walkTimer);
    }

    void RunHandler()
    {
        runTimer += Time.deltaTime;
        Debug.Log("RUNNING: " + runTimer);
    }
}
