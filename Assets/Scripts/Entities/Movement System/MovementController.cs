using UnityEngine;

[RequireComponent(typeof(Movement))]
public abstract class MovementController : MonoBehaviour
{
    protected SpatialArray<Tiles> _map;
    public bool HasReceivedMap { get; protected set; }

    //protected StateMachine _stateMachine;
    //public MovementState MovementState { get => _movementState; set => _movementState = value; }
    protected Transform _tr;
     
    protected Movement _movement;

    protected Vector2 _moveInput;
    public Vector2 MoveInput { get => _moveInput; protected set => _moveInput = value; }
    protected bool _jumpInput;
    public bool JumpInput { get => _jumpInput; protected set => _jumpInput = value; }
    public Vector2 Impulse { get; protected set; }

    protected virtual void Awake()
    {
        HasReceivedMap = false;
        //_stateMachine = _movement._stateMachine;

    }

    protected virtual void Start()
    {
        _movement = GetComponent<Movement>();   
    }

    protected void FixedUpdate()
    {
        if (!HasReceivedMap)
        {
            return;
        }
        
        _movement.Move(MoveInput, JumpInput, Impulse);

        // reset inputs
        JumpInput = false;
        Impulse = Vector2.zero;
    }

    protected abstract void Update();

    public void AddImpulse(Vector2 add)
    {
        Impulse += add;
    }

    protected virtual void AssignMap(SpatialArray<Tiles> map)
    {
        _map = map;
        HasReceivedMap = true;
    }

    protected void OnEnable()
    {
        GeneratorManager.WorldLoaded += AssignMap;
        
    }

    protected void OnDisable()
    {
        GeneratorManager.WorldLoaded -= AssignMap;
    }
}

