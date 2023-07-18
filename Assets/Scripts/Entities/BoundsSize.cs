using UnityEngine;

[CreateAssetMenu(fileName = "New Bounds Size", menuName = "Scriptable Objects/Bounds Size")]
public class BoundsSize : ScriptableObject
{
    [field: SerializeField] public Vector2 Min { get; private set; }
    [field: SerializeField] public Vector2 Max { get; private set; }
}
