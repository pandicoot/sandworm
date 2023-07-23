using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyWorld : MonoBehaviour
{
    public Transform Transform { get; private set; }
    [field: SerializeField] public float Range { get; private set; }
    [field: SerializeField] public ChunkManager ChunkManager { get; private set; }

    private SpatialArray<Tiles>[] _layers { get; set; }
    private bool _hasReceivedMap { get; set; }

    public event Action<GeneratorManager.TileLayerIndices, Vector2, HashSet<(Vector2Int, Tiles)>> BuiltAt;
    public event Action<GeneratorManager.TileLayerIndices, Vector2, HashSet<(Vector2Int, Tiles)>> DestroyedAt;

    private void Awake()
    {
        _hasReceivedMap = false;
    }

    private void Start()
    {
        Transform = transform;
    }

    public int Build(Vector2 positionToBuildAt, GeneratorManager.TileLayerIndices layerIndex, ICarve tool, Tiles tileToBuildWith, int limit)
    {
        //Debug.Log($"{positionToBuildAt}, {layerIndex}, {tool}, {tileToBuildWith}");

        if (!_hasReceivedMap)
        {
            return 0;
        }

        if (Vector2.SqrMagnitude(positionToBuildAt - (Vector2)Transform.position) > Range * Range)
        {
            return 0;
        }
        //Debug.Log(positionToBuildAt);
        //Debug.Log("Map position: " + ChunkManager.ToMapPosition(positionToBuildAt));
        GizmoAtMapPos.DrawGizmoAt(positionToBuildAt);

        var affectedTiles = tool.Carve(positionToBuildAt, _layers[(int)layerIndex], tileToBuildWith, TileManager.Air, limit);

        var affectedChunks = new HashSet<Vector2Int>();
        foreach ((var pos, var tile) in affectedTiles)
        {
            affectedChunks.Add(ChunkManager.ConvertToChunkPositionInBounds(ChunkManager.ToWorldPosition(pos)));
        }

        // regenerate affected chunks
        foreach (Vector2Int chunkCoord in affectedChunks)
        {
            ChunkManager.RegenerateChunkLayerAt(chunkCoord, layerIndex);
        }

        BuiltAt?.Invoke(layerIndex, positionToBuildAt, affectedTiles);
        return affectedTiles.Count;
    }

    public void Destruct(Vector2 positionToDestructAt, GeneratorManager.TileLayerIndices layerIndex, ICarve tool)
    {
        //Debug.Log($"{positionToDestructAt}, {layerIndex}, {tool}");
        if (!_hasReceivedMap)
        {
            return;
        }

        //Debug.Log(Vector2.SqrMagnitude(positionToDestructAt - (Vector2)Transform.position));
        if (Vector2.SqrMagnitude(positionToDestructAt - (Vector2)Transform.position) > Range * Range)
        {
            return;
        }

        var affectedTiles = tool.Carve(positionToDestructAt, _layers[(int)layerIndex], Tiles.Air, TileManager.AllPhysicalTiles );  // tileDomain should be tool-based, and maybe other properties

        var affectedChunks = new HashSet<Vector2Int>();
        foreach (( var pos, var tile ) in affectedTiles)
        {
            affectedChunks.Add(ChunkManager.ConvertToChunkPositionInBounds(ChunkManager.ToWorldPosition(pos)));
        }

        // regenerate affected chunks
        foreach (Vector2Int chunkCoord in affectedChunks)
        {
            ChunkManager.RegenerateChunkLayerAt(chunkCoord, layerIndex);
        }

        DestroyedAt?.Invoke(layerIndex, positionToDestructAt, affectedTiles);
    }

    private void AssignLayers(SpatialArray<Tiles>[] layers)
    {
        _layers = layers;
        _hasReceivedMap = true;
    }

    private void OnEnable()
    {
        GeneratorManager.AllLayersLoaded += AssignLayers;
    }

    private void OnDisable()
    {
        GeneratorManager.AllLayersLoaded -= AssignLayers;
    }
}
