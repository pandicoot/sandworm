using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMovementState : MovementState
{
    public override bool CanStep => false;

    protected override void UpdateXVel(ref float currXVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currXVel = 0;
    }

    protected override void UpdateYVel(ref float currYVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currYVel = 0;
    }
}
