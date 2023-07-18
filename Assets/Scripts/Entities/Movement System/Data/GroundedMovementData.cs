using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Grounded Movement Data", menuName = "Scriptable Objects/Entity/Movement/Grounded")]
public class GroundedMovementData : ScriptableObject
{
    [field: SerializeField] public Vector2 MaxSpeed { get; private set; }
    [field: SerializeField] public Vector2 AccelerationSmoothTime { get; private set; }
    [field: SerializeField] public float JumpImpulse { get; private set; }
}
