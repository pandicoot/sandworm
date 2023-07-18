using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMovementAIState : MovementAIState
{
    public static float GrowthParameter = 0.1f;

    public float MinTimeInOneMotion { get; set; }
    public float DirectionChangeProbability { get; set; }

    public IdleMovementAIState(float walkSpeedFr, float runSpeedFr, JumpController jc, float minTimeInOneMotion, float directionChangePr) : base(walkSpeedFr, runSpeedFr, jc)
    {
        MinTimeInOneMotion = minTimeInOneMotion;
        DirectionChangeProbability = directionChangePr;
    }

    private bool ChooseRandomDirection(float minTime, float currentTime, float basePr)
    {
        Vector2 newDir = Direction;
        if (currentTime < minTime)
        {
            return false;
        }

        float adjustedPr = 1 - ((1 - basePr) / (GrowthParameter * (currentTime - minTime) + 1));

        //Debug.Log("curr time: " + currentTime);
        //Debug.Log("adj pr: " + adjustedPr);
        

        if (Random.value < adjustedPr)
        {
            var dirRoll = Random.Range(0, 4);
            if (dirRoll == 0 || dirRoll == 1)
            {
                newDir = Vector2.zero;
            }
            else if (dirRoll == 2)
            {
                newDir = Vector2.left;
            }
            else
            {
                newDir = Vector2.right;
            }
        }

        if (newDir == Direction)
        {
            return false;
        }
        else
        {
            Direction = newDir;
            return true;
        }
    }

    private void UpdateTranslation(ref Vector2 moveInput)
    {
        if (ChooseRandomDirection(MinTimeInOneMotion, TimeOfCurrentMotion, DirectionChangeProbability))
        {
            TimeOfCurrentMotion = 0;
        }

        moveInput = Direction * WalkSpeedFraction;
    }

    public override void UpdateInput(ref Vector2 moveInput, ref bool jumpInput)
    {
        JumpController.UpdateJump(ref jumpInput, Direction);
        UpdateTranslation(ref moveInput);

        TimeOfCurrentMotion += Time.deltaTime;
    }


}
