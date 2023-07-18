using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackerController : AttackerController
{
    //private void OnEnable()
    //{
    //    InputManager.Actions.gameplay.primary.performed += Primary_performed;
    //}
    //private void OnDisable()
    //{
    //    InputManager.Actions.gameplay.primary.performed -= Primary_performed;
    //}

    //private void Primary_performed(InputAction.CallbackContext obj)
    //{
    //    TryAttack();
    //}

    protected override void GetAttackInput()
    {
        AttackInput = (InputManager.Actions.gameplay.primary.IsPressed());
        
    }
}
