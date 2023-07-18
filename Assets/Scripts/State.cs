using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    public HashSet<State> Transitions { get; protected set; }

    public void AddTransition(State to)
    {
        Transitions.Add(to);
    }

    public State()
    {
        Transitions = new HashSet<State>();
    }
}
