using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileCollider))]
public class CharacterMovement : Movement  // IMovable defines UpdateVelocity as well?
{
    protected MovementState _groundedState;
    protected MovementState _midairState;

    [field: SerializeField] protected GroundedMovementData GroundedMovementData { get; set; }
    [field: SerializeField] protected CharacterMidairMovementData CharacterMidairMovementData { get; set; }

    //[field: SerializeField] public float AccelerationX { get; private set; }
    //[field: SerializeField] public float AccelerationY { get; private set; }
    //[field: SerializeField] public AnimationCurve AccelerationResponseCurve { get; private set; }
    //[field: SerializeField] public float FullResponseTime { get; private set; }

    // array of directed inputs, clockwise from up
    //private BitArray _inputs = new BitArray(4);
    //private float[] _sustainedInputTimes = new float[4];
    // array of contacting edges of AABB, clockwise from top
    //private BitArray _contacts = new BitArray(4);  // BitVector32 is faster

    protected override void SetupStateMachine()
    {
        // set up state machine
        _groundedState = new GroundedMovementState(GroundedMovementData);
        _midairState = new CharacterMidairMovementState(CharacterMidairMovementData);  // different

        _stateMachine = new StateMachine(_groundedState, _midairState);

        _stateMachine.AddTransition(_groundedState, _midairState);
        _stateMachine.AddTransition(_midairState, _groundedState);
    }

    protected override void UpdateState()
    {
        //Debug.Log(_tr);
        //Debug.Log(_bnds);
        //Debug.Log(_map);
        //Debug.Log(_stateMachine);

        //Vector2 feetPosMap = ChunkManager.ToMapPosition(new Vector2(_tr.position.x, _tr.position.y - Bounds.extents.y));
        float feetYMap = ChunkManager.ToMapPosition(Transform.position.y - Bounds.extents.y);
        int leftXMap = ChunkManager.ToBlockPosition(ChunkManager.ToMapPosition(Transform.position.x - Bounds.extents.x));
        int rightXMap = ChunkManager.ToBlockPosition(ChunkManager.ToMapPosition(Transform.position.x + Bounds.extents.x));
        //Debug.Log($"feet Y (map): {feetYMap}");
        bool grounded = false;

        for (var x = leftXMap; x <= rightXMap; x++)
        {
            if (_map.Get(x, ChunkManager.ToBlockPosition(feetYMap - TileCollider.CollisionBuffer * 2)) != Tiles.Air)
            {
                grounded = true;
                break;
            }
        }

        if (grounded)
        {
            _stateMachine.Transition(_groundedState);
        }
        else
        {
            _stateMachine.Transition(_midairState);
        }
    }

    //private void UpdateInputTimes(Vector2 input, float deltaT)
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        _inputs[i] = false;
    //    }

    //    if (input.x > 0)
    //    {
    //        _inputs[1] = true;
    //    }
    //    if (input.x < 0)
    //    {
    //        _inputs[3] = true;
    //    }
    //    if (input.y > 0)
    //    {
    //        _inputs[0] = true;
    //    }
    //    if (input.y < 0)
    //    {
    //        _inputs[2] = true;
    //    }

    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (!_inputs[i])
    //        {
    //            _sustainedInputTimes[i] = 0f;
    //        }
    //        else
    //        {
    //            _sustainedInputTimes[i] += deltaT;
    //        }
    //    }
    //}

    //private int GetContactIndex(bool isX, int dir)
    //{
    //    int res = -1;
    //    if (dir == 0)
    //    {
    //        return res;
    //    }

    //    if (isX)
    //    {
    //        if (dir > 0)
    //        {
    //            res = 1;
    //        }
    //        else
    //        {
    //            res = 3;
    //        }
    //    }
    //    else
    //    {
    //        if (dir > 0)
    //        {
    //            res = 0;
    //        }
    //        else
    //        {
    //            res = 2;
    //        }
    //    }

    //    return res;
    //}

    // deprecated
    //private void AdditiveUpdateVelocity(Vector2 input)
    //{
    //    UpdateInputTimes(input, Time.fixedDeltaTime);
    //    //Debug.Log($"({_sustainedInputTimes[0]}, {_sustainedInputTimes[1]}, {_sustainedInputTimes[2]}, {_sustainedInputTimes[3]})");

    //    if (input.x > 0)
    //    {
    //        velocity.x = Mathf.Min(MaxSpeedX, velocity.x + AccelerationX * AccelerationResponseCurve.Evaluate(Mathf.Clamp01(_sustainedInputTimes[1] / FullResponseTime)) * Time.fixedDeltaTime);
    //    }
    //    else if (input.x < 0)
    //    {
    //        velocity.x = Mathf.Max(-MaxSpeedX, velocity.x - AccelerationX * AccelerationResponseCurve.Evaluate(Mathf.Clamp01(_sustainedInputTimes[3] / FullResponseTime)) * Time.fixedDeltaTime);
    //    }

    //    if (input.y > 0)
    //    {
    //        velocity.y = Mathf.Min(MaxSpeedY, velocity.y + AccelerationY * AccelerationResponseCurve.Evaluate(Mathf.Clamp01(_sustainedInputTimes[0] / FullResponseTime)) * Time.fixedDeltaTime);
    //    }
    //    else if (input.y < 0)
    //    {
    //        velocity.y = Mathf.Max(-MaxSpeedY, velocity.y - AccelerationY * AccelerationResponseCurve.Evaluate(Mathf.Clamp01(_sustainedInputTimes[2] / FullResponseTime)) * Time.fixedDeltaTime);
    //    }
    //}
}
