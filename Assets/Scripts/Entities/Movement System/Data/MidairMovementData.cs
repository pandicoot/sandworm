using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Midair Movement Data", menuName = "Scriptable Objects/Entity/Movement/Midair")]
public class MidairMovementData : ScriptableObject
{
    [field: SerializeField] public Vector2 MaxSpeed { get; private set; }
    [field: SerializeField] public float AirDrag { get; private set; }
    [field: SerializeField] public float GravityResponseFactor { get; private set; }
}
