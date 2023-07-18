using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasSizer : MonoBehaviour
{
    private RectTransform _rt { get; set; }

    private void Start()
    {
        _rt = GetComponent<RectTransform>();
        _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ChunkManager.TileSize * GeneratorManager.LengthInChunkUnits * ChunkManager.ChunkSize.x);
        _rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ChunkManager.TileSize * GeneratorManager.HeightInChunkUnits * ChunkManager.ChunkSize.y);

    }
}
