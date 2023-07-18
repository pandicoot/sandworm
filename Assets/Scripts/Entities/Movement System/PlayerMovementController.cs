using System;
using UnityEngine;

public class PlayerMovementController : MovementController
{
    private void UpdateTranslation()
    {
        //MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        MoveInput = InputManager.Actions.gameplay.move.ReadValue<Vector2>();
    }

    private void UpdateJump()
    {
        //if (Input.GetButtonDown("Jump")) {
        //    JumpInput = true;
        //}

        // scuffed, fix later to a callback system if we can. maybe unnecessary
        if (InputManager.Actions.gameplay.jump.WasPressedThisFrame())
        {
            JumpInput = true;
        }
    }

    protected override void Update()
    {
        UpdateTranslation();
        UpdateJump();
    }
}
