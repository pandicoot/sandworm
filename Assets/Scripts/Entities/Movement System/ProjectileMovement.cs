using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : Movement
{
    protected ProjectileMidairMovementState _midairState;
    protected EmptyMovementState _affixedState;

    [field: SerializeField] protected MidairMovementData MidairMovementData { get; set; }

    protected override void Start()
    {
        Bounds = new Bounds(Vector2.negativeInfinity, Vector2.zero);
        Collider = null;
    }

    protected override void SetupStateMachine()
    { 
        _affixedState = new EmptyMovementState();
        _midairState = new ProjectileMidairMovementState(MidairMovementData); 

        _stateMachine = new StateMachine(_midairState, _affixedState);

        _stateMachine.AddTransition(_midairState, _affixedState);
        _stateMachine.AddTransition(_affixedState, _midairState);
    }

    protected override void UpdateState()
    {
        var mapPos = ChunkManager.ToBlockPosition(ChunkManager.ToMapPosition(Transform.position));
        //Debug.Log(mapPos);

        if (_map.Get(mapPos.x, mapPos.y) != Tiles.Air)
        {
            _stateMachine.Transition(_affixedState);
        }
        else
        {
            _stateMachine.Transition(_midairState);
        }

        //Debug.Log(_stateMachine.CurrentState);
    }
}
