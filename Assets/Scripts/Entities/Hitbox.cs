using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [field: SerializeField] public BoundsSize BoundsSize { get; private set; }

    public Vector2 Min { get => BoundsSize.Min; }
    public Vector2 Max { get => BoundsSize.Max; }

    public Vector2 Size { get => Max - Min; }
    public Bounds Bounds { get => new Bounds(transform.position, Size); }

    public void OnDrawGizmos()
    {
        var bl = (Vector2)transform.position + Min;
        var tr = (Vector2)transform.position + Max;
        Gizmos.DrawLine(bl, bl + new Vector2(0, Size.y));
        Gizmos.DrawLine(bl + new Vector2(0, Size.y), tr);
        Gizmos.DrawLine(tr, tr - new Vector2(0, Size.y));
        Gizmos.DrawLine(tr - new Vector2(0, Size.y), bl);
    }
}
