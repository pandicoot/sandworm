using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CustomRaycaster : MonoBehaviour
{
    //[SerializeField] private bool Do2D;

    private void OnEnable()
    {
        InputManager.Actions.gameplay.do_raycast.performed += InputCast;
        InputManager.Actions.gameplay.show_pointer_over_game_obj.performed += PointerOverGameObject;
    }

    private void OnDisable()
    {
        InputManager.Actions.gameplay.do_raycast.performed -= InputCast;
        InputManager.Actions.gameplay.show_pointer_over_game_obj.performed -= PointerOverGameObject;
    }

    private void InputCast(InputAction.CallbackContext ctx) => Cast();

    public void Cast()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                Debug.Log($"Hit: {hit.collider.gameObject.name}; Target Position: " + hit.collider.gameObject.transform.position);
            }
        }
        return;

        //var ray = Camera.main.ScreenPointToRay(InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>());
        //Debug.DrawRay(ray.origin, ray.direction, Color.white, 5f);
        //Debug.Log($"trying to cast: from {ray.origin} in {ray.direction}");
        //RaycastHit2D[] hits = Physics2D.RaycastAll(ray, Vector2.zero);
        //Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        //Array.ForEach(hits, hit => Debug.Log(hit.transform.name));
    }

    public void PointerOverGameObject(InputAction.CallbackContext ctx)
    {
        Debug.Log(EventSystem.current.IsPointerOverGameObject());
    }
}
