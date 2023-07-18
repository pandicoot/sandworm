using UnityEngine;

public class CharacterMidairMovementState : ProjectileMidairMovementState
{
    protected override void UpdateXVel(ref float currXVel, Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        currXVel = Mathf.SmoothDamp(currXVel, translationInput.x * MaxSpeedX, ref _smoothingVelocity.x, AccelerationSmoothTime.x) + impulse.x;
    }

    public CharacterMidairMovementState(CharacterMidairMovementData data) : base(data)
    {
        InitBaseParams(data.MaxSpeed, new Vector2(data.AirDrag * data.StrafeSmoothTime, Mathf.Infinity));
    }
}
