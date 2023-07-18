using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private HashSet<State> States = new HashSet<State>();
    public State CurrentState { get; private set; }

    public StateMachine(params State[] states)
    {
        foreach (var s in states)
        {
            States.Add(s);
        }

        CurrentState = states[0];
    }

    public void AddTransition(State from, State to)
    {
        if (from == null || to == null)
        {
            return;
        }
        if (!States.Contains(from) || !States.Contains(to))
        {
            return;
        }

        from.AddTransition(to);
    }

    public void Transition(State to)
    {
        if (to == null)
        {
            return;
        }
        // self loops allowed by default
        if (to == CurrentState)
        {
            return;
        }

        if (!CurrentState.Transitions.Contains(to))
        {
            return;
        }

        CurrentState = to;
    }
}
