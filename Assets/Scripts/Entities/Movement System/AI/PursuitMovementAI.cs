using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitMovementAIState : MovementAIState
{
    [field: SerializeField] public Transform Target { get; set; }
    public Transform Transform { get; protected set; }
    public float RestDistance { get; protected set; }
    public float MaxRange { get; protected set; }

    public Vector2 CurrentDisplacement { get; protected set; }

    public PursuitMovementAIState(float walkSpeedFr, float runSpeedFr, JumpController jc, Transform transform, float restDistance, float maxRange) : base(walkSpeedFr, runSpeedFr, jc)
    {
        Transform = transform;
        RestDistance = restDistance;
        MaxRange = maxRange;
    }


    public override void UpdateInput(ref Vector2 moveInput, ref bool jumpInput)
    {
        CurrentDisplacement = Target.position - Transform.position;
        if (CurrentDisplacement.sqrMagnitude > MaxRange * MaxRange)
        {
            Direction = Vector2.zero;
            // reset target
            // transition back to idle state
        }
        else if (CurrentDisplacement.sqrMagnitude < RestDistance * RestDistance)
        {
            Direction = Vector2.zero;
        }
        else
        {
            //var adjustedDisplacement = CurrentDisplacement - CurrentDisplacement.normalized * RestDistance;
            //Debug.DrawLine(Transform.position, Transform.position + (Vector3)CurrentDisplacement);
            //Debug.DrawLine(Transform.position, Transform.position + (Vector3)adjustedDisplacement, Color.yellow);
            //Direction = adjustedDisplacement.normalized;
            Direction = CurrentDisplacement.normalized;
        }
        JumpController.UpdateJump(ref jumpInput, Direction);
        moveInput = Direction;
    }
}
