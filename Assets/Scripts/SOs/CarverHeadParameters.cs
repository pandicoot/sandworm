using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Carver Head Parameters", menuName = "Scriptable Objects/Carver Head Parameters")]
public class CarverHeadParameters : ScriptableObject
{
    [field: SerializeField] public float MinCarvingRadius { get; private set; }
    [field: SerializeField] public float CarvingRadiusMaxSpread { get; private set; }
    [field: SerializeField] public NoiseParameters CarvingNoiseParameters { get; private set; }
    [field: SerializeField] public AnimationCurve CarvingNoiseCurve { get; private set; }

    public float MaxRadius { get => MinCarvingRadius + CarvingRadiusMaxSpread; }
}
