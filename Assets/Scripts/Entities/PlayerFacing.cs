
using UnityEngine;

public class PlayerFacing : Facing
{
    private Camera _camera;
    private Transform _transform;

    protected override void Start()
    {
        base.Start();
        _camera = gameObject.GetComponentInChildren<Camera>();
        _transform = transform;
    }

    protected override Vector2 GetNewFacingDirection()
    {
        return ((Vector3)InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>() - _camera.WorldToScreenPoint(_transform.position)).normalized;
    }

    
}
