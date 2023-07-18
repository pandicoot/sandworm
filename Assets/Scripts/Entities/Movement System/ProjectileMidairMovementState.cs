using UnityEngine;

public class ProjectileMidairMovementState : MovementState
{
    public float GravitationalAcceleration { get; set; }

    public override bool CanStep => false;

    protected override void UpdateXVel(ref float currXVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        // TODO: drag
        currXVel += impulse.x;
    }

    protected override void UpdateYVel(ref float currYVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currYVel = Mathf.Clamp(currYVel - GravitationalAcceleration * Time.fixedDeltaTime, -MaxSpeedY, MaxSpeedY) + impulse.y;
    }

    public ProjectileMidairMovementState(MidairMovementData data)
    {
        InitBaseParams(data.MaxSpeed, new Vector2(Mathf.Infinity, Mathf.Infinity));

        GravitationalAcceleration = data.GravityResponseFactor * Movement.Gravity;
    }
}
