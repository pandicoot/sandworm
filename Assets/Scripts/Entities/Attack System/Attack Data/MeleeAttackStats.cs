using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackStats : Prototype
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Knockback { get; private set; }
    [field: SerializeField] public float SwingSpeed { get; private set; }
    [field: SerializeField] public float ArcAngle { get; private set; }
}
