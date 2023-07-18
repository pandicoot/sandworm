using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MovementState : State
{
    public float MaxSpeedX { get; set; }
    public float MaxSpeedY { get; set; }
    public Vector2 AccelerationSmoothTime { get; set; }
    protected Vector2 _smoothingVelocity;
    public abstract bool CanStep { get; }

    protected virtual void UpdateXVel(ref float currXVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currXVel = Mathf.SmoothDamp(currXVel, translationInput.x * MaxSpeedX, ref _smoothingVelocity.x, AccelerationSmoothTime.x) + impulse.x;
    }
    protected virtual void UpdateYVel(ref float currYVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currYVel = Mathf.SmoothDamp(currYVel, translationInput.y * MaxSpeedY, ref _smoothingVelocity.y, AccelerationSmoothTime.y) + impulse.y;
    }

    public void UpdateVel(ref Vector2 currVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        UpdateXVel(ref currVel.x, translationInput, jumpInput, impulse);
        UpdateYVel(ref currVel.y, translationInput, jumpInput, impulse);
    }

    protected void InitBaseParams(Vector2 maxSpeed, Vector2 accelSmoothTime)  // TODO move to constructor?
    {
        MaxSpeedX = maxSpeed.x;
        MaxSpeedY = maxSpeed.y;
        AccelerationSmoothTime = accelSmoothTime;
    }

    public MovementState()
    {
    }
}
