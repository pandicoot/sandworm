using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "New Carver", menuName = "Scriptable Objects/Carver")]
public class CarverParameters : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public int StepDistance { get; set; }
    [field: SerializeField] public int MeanNodeDistance { get; set; }
    [field: SerializeField] public int NodeDistanceSpread { get; set; }

    [SerializeField] private float _meanTurningAngle;
    public float MeanTurningAngle
    {
        get => _meanTurningAngle;
        set => _meanTurningAngle = value % (2 * Mathf.PI);
    }
    [SerializeField] private float _turningAngleSpread;
    public float TurningAngleSpread
    {
        get => _turningAngleSpread;
        set => _turningAngleSpread = value % (2 * Mathf.PI);
    }

    //[field: SerializeField] public float MinCarvingRadius { get; set; }
    //[field: SerializeField] public float CarvingRadiusMaxSpread { get; set; }

    [SerializeField] private float _deathProbability;
    public float DeathProbability
    {
        get => _deathProbability;
        set => _deathProbability = Mathf.Clamp01(value);
    }

    [field: SerializeField] public float CarverHeadPositionSpread { get; set; }

    //public NoiseParameters CarvingNoiseParameters;
    //public AnimationCurve CarvingNoiseCurve;
}
