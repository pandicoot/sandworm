using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MovementAIState : State
{
    public float WalkSpeedFraction { get; protected set; }
    public float RunSpeedFraction { get; protected set; }
    protected float TimeOfCurrentMotion { get; set; }
    public Vector2 Direction { get; protected set; }
    public JumpController JumpController { get; protected set; }


    public abstract void UpdateInput(ref Vector2 moveInput, ref bool jumpInput);


    public MovementAIState(float walkSpeedFr, float runSpeedFr , JumpController jc)
    {
        WalkSpeedFraction = walkSpeedFr;
        RunSpeedFraction = runSpeedFr;
        JumpController = jc;
        TimeOfCurrentMotion = 0;
    }
}
