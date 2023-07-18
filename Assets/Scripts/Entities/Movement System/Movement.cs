using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    private static float s_gravity = 50f;
    public static float Gravity { get => s_gravity; private set => s_gravity = value; }

    public Transform Transform { get; protected set; }
    public Bounds Bounds { get; protected set; }
    protected TileCollider Collider { get; set; }

    protected SpatialArray<Tiles> _map;
    public bool HasReceivedMap { get; protected set; }

    public StateMachine _stateMachine { get; set; }

    protected Vector2 _smoothingVelocity;
    protected Vector2 _velocity;
    public Vector2 Velocity { get => _velocity; protected set => _velocity = value; }

    protected abstract void SetupStateMachine();

    protected void Awake()
    {
        Transform = transform;
        HasReceivedMap = false;

        SetupStateMachine();
    }

    protected void OnEnable()
    {
        GeneratorManager.WorldLoaded += AssignMap;
    }

    protected void OnDisable()
    {
        GeneratorManager.WorldLoaded -= AssignMap;
    }

    private void AssignMap(SpatialArray<Tiles> map)
    {
        _map = map;
        HasReceivedMap = true;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Bounds = GetComponent<Hitbox>().Bounds;
        Collider = GetComponent<TileCollider>();
    }

    protected abstract void UpdateState();

    public void Move(Vector2 translationInput, bool jumpInput, Vector2 impulse)
    {
        if (!HasReceivedMap)
        {
            return;
        }

        UpdateState();  
        var currState = (MovementState)_stateMachine.CurrentState;
        //Debug.Log(currState);
        currState.UpdateVel(ref _velocity, translationInput, jumpInput, impulse);

        // step position
        Vector2 oPos = Transform.position;
        Vector2 targetPos = oPos + Velocity * Time.fixedDeltaTime;
        if (Collider != null)
        {
            Transform.position = Collider.GetPositionAccessible(targetPos, oPos, ref _velocity, Bounds.extents, currState.CanStep);
        }
        else
        {
            Transform.position = targetPos;
        }
    }
}
