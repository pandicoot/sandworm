using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyOverlay : MonoBehaviour
{
    [SerializeField] private Transform _overlayObject;
    private SpriteRenderer _sr;
    private Transform OverlayObject { get => _overlayObject; set => _overlayObject = value; }

    private void Start()
    {
        _sr = _overlayObject.GetComponent<SpriteRenderer>();
    }

    public void UpdatePosition(Vector2 worldPosRaw)
    {
        var worldPosSnapped = ChunkManager.ToWorldPosition(ChunkManager.ToBlockPosition(ChunkManager.ToMapPosition(worldPosRaw)));
        _overlayObject.position = new Vector3 (worldPosSnapped.x, worldPosSnapped.y, _overlayObject.position.z);
    }

    public void UpdateSize(Vector2Int size)
    {
        _sr.size = size;
    }
}
