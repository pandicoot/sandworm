using UnityEngine;
public class GroundedMovementState : MovementState
{
    public float JumpImpulse { get; set; }

    public override bool CanStep => true;

    //protected override void UpdateXVel(ref float currXVel, Vector2 input)
    //{
    //    currXVel = Mathf.SmoothDamp(currXVel, input.x * MaxSpeedX, ref _smoothingVelocity.x, AccelerationSmoothTime.x);
    //}

    protected override void UpdateYVel(ref float currYVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        if (jumpInput)
        {
            currYVel = JumpImpulse;
        }

        currYVel = Mathf.SmoothDamp(currYVel, 0, ref _smoothingVelocity.y, AccelerationSmoothTime.y) + impulse.y;
    }

    public GroundedMovementState(GroundedMovementData data) 
    {
        InitBaseParams(data.MaxSpeed, data.AccelerationSmoothTime);

        JumpImpulse = data.JumpImpulse;
    }
}
