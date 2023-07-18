using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementAI : MovementController
{
    protected StateMachine AIStateMachine { get; set; }
    protected IdleMovementAIState IdleState { get; set; }
    protected PursuitMovementAIState PursuitState { get; set; }

    private JumpController _jc;

    [field: SerializeField] protected float WalkSpeedFraction { get; set; }
    [field: SerializeField] protected float RunSpeedFraction { get; set; }
    [field: SerializeField] protected float MinTimeInOneMotion { get; set; }
    [field: SerializeField] protected float DirectionChangeBaseProbability { get; set; }

    [field: SerializeField] protected float PursuitRestDistance { get; set; }
    [field: SerializeField] protected float PursuitMaxRange { get; set; }

    public bool PursuePlayer;

    protected override void Start()
    {
        base.Start();
        // set up state machine
        _jc = new JumpController(null, _movement.Bounds.extents, transform);
        IdleState = new IdleMovementAIState(WalkSpeedFraction, RunSpeedFraction, _jc, MinTimeInOneMotion, DirectionChangeBaseProbability);
        PursuitState = new PursuitMovementAIState(WalkSpeedFraction, RunSpeedFraction, _jc, transform, PursuitRestDistance, PursuitMaxRange);
        PursuitState.Target = GameObject.FindGameObjectWithTag("Player").transform;
        AIStateMachine = new StateMachine(IdleState, PursuitState);

        AIStateMachine.AddTransition(IdleState, PursuitState);
        AIStateMachine.AddTransition(PursuitState, IdleState);
    }

    protected void UpdateState()
    {
        if (PursuePlayer)
        {
            AIStateMachine.Transition(PursuitState);
        }
        else
        {
            AIStateMachine.Transition(IdleState);
        }
    }

    protected override void Update()
    {
        if (!HasReceivedMap)
        {
            return;
        }

        UpdateState();
        var currState = (MovementAIState)AIStateMachine.CurrentState;
        currState.UpdateInput(ref _moveInput, ref _jumpInput);
        //Debug.Log($"{MoveInput}, {JumpInput}");
    }

    protected override void AssignMap(SpatialArray<Tiles> map)
    {
        base.AssignMap(map);
        _jc.Map = map;
    }
}
