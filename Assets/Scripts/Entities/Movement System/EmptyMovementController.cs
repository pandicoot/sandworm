
using UnityEngine;

public class EmptyMovementController : MovementController
{
    protected override void Update()
    {
        MoveInput = Vector2.zero;
    }
}
