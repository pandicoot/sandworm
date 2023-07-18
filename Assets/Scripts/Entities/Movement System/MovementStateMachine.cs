using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementStateID
{
    Grounded,
    Midair
}

public class MovementStateMachine 
{
    //Dictionary<MovementStateID, MovementState> states = new Dictionary<MovementStateID, MovementState>();

    //public MovementState CurrentState { get; private set; }

    //public MovementStateMachine(params MovementState[] states)
    //{
    //    foreach (var s in states)
    //    {
    //        this.states.Add(s.ID, s);
    //    }

    //    CurrentState = states[0];
    //}

    //public void AddTransition(MovementStateID from, MovementStateID to)
    //{
    //    states.TryGetValue(from, out MovementState fromState);
    //    if (fromState == null)
    //    {
    //        return;
    //    }

    //    states.TryGetValue(from, out MovementState toState);
    //    if (toState == null)
    //    {
    //        return;
    //    }

    //    fromState.AddTransition(to);

    //}

    //public void Transition(MovementStateID to)
    //{
    //    // self loops allowed by default
    //    if (to == CurrentState.ID)
    //    {
    //        return;
    //    }

    //    if (!states.ContainsKey(to))
    //    {
    //        return;
    //    }

    //    if (!CurrentState.Transitions.Contains(to))
    //    {
    //        return;
    //    }

    //    CurrentState = states[to];
    //}
}
