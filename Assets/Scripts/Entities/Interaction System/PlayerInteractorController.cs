using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractorController : InteractorController
{
    private Camera _camera;

    protected override void Start()
    {
        base.Start();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        //Vector2 mousePos;
        //if (Input.GetButton("MouseRight"))
        //{
        //    mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        //    foreach (Interactable interactable in Interactor.InteractablesInRange)
        //    {
        //        if (interactable.Bounds.Contains(mousePos))
        //        {
        //            Interactor.InteractWith(interactable);
        //            break;
        //        }
        //    }
        //}
    }
}
