using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Midair Movement Data", menuName = "Scriptable Objects/Entity/Movement/Midair Character")]
public class CharacterMidairMovementData : MidairMovementData
{
    [field: SerializeField] public float StrafeSmoothTime { get; private set; }
}
