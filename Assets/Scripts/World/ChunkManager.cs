using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(TileMeshGenerator), typeof(TileTextureGenerator))]
public class ChunkManager : MonoBehaviour
{
    public const float TileSize = 0.25f;
    private static readonly Vector2Int _chunkSize = new Vector2Int(64, 64);
    public static Vector2Int ChunkSize
    {
        get => _chunkSize;
    }


    [SerializeField] private GeneratorManager _worldGen;
    [SerializeField] private TileMeshGenerator _meshGen;
    [SerializeField] private TileTextureGenerator _texGen;

    [SerializeField] private Camera _viewer;  // potentially have a LIST of viewers and associated properties
    private Transform _viewerTransform;
    [SerializeField] private int _bufferViewDist;

    private SpatialArray<Chunk> _chunks;
    public SpatialArray<Chunk> Chunks { get => _chunks; private set => _chunks = value; }

    private float _viewerHorizontalHalfSpan;  // vectorise
    private float _viewerVerticalHalfSpan;

    public bool showChunkAnchors;
    public bool showAllInstantiatedChunks;

    void Start()
    {
        _chunks = new SpatialArray<Chunk>(GeneratorManager.LengthInChunkUnits, GeneratorManager.HeightInChunkUnits);
        //_chunks = new Chunk[_mapGen.LengthInChunkUnits * _mapGen.HeightInChunkUnits];

        _viewerTransform = _viewer.transform;
        _viewerVerticalHalfSpan = _viewer.orthographicSize;
        _viewerHorizontalHalfSpan = _viewer.aspect * _viewerVerticalHalfSpan;

        _worldGen.Generate();
    }

    private static Vector2Int ConvertRealPositionToChunkPosition(Vector2 position) // use roundToInt instead????
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / (ChunkSize.x * TileSize)), Mathf.FloorToInt(position.y / (ChunkSize.y * TileSize)));
    }

    public static Vector2Int ConvertToChunkPositionInBounds(Vector2 position)
    {
        Vector2Int chunkPos = ConvertRealPositionToChunkPosition(position);
        chunkPos.x = Mathf.Clamp(chunkPos.x, 0, GeneratorManager.LengthInChunkUnits - 1);
        chunkPos.y = Mathf.Clamp(chunkPos.y, 0, GeneratorManager.HeightInChunkUnits - 1);

        return chunkPos;
    }

    public static float ToMapPosition(float worldCoord)
    {
        return worldCoord / TileSize;
    }

    public static Vector2 ToMapPosition(Vector2 worldPosition)
    {
        return worldPosition / TileSize;
    }

    public static float ToWorldPosition(float mapCoord)
    {
        return mapCoord * TileSize;
    }

    public static Vector2 ToWorldPosition(Vector2 mapPosition)
    {
        return mapPosition * TileSize;
    }
    public static Vector2Int ToBlockPosition(Vector2 mapPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(mapPosition.x), Mathf.FloorToInt(mapPosition.y));
    }
    public static int ToBlockPosition(float mapOrd)
    {
        return Mathf.FloorToInt(mapOrd);
    }


    public void UpdateChunks()
    {
        for (int i = 0; i < GeneratorManager.LengthInChunkUnits * GeneratorManager.HeightInChunkUnits; i++)
        {
            if (_chunks.Get(i) != null)
            {
                _chunks.Get(i).SetAllVisible(showAllInstantiatedChunks);
                //_chunks.Get(i).SetVisibleAnchor(showChunkAnchors);
            }
        }

        Vector2 currentViewerPos = _viewerTransform.position;
        Vector2Int currentViewerChunkPos = ConvertRealPositionToChunkPosition(currentViewerPos);

        var topLeftViewCornerDisplacement = new Vector2(-1 * _viewerHorizontalHalfSpan, _viewerVerticalHalfSpan);
        Vector2Int topLeftChunkCoord = ConvertToChunkPositionInBounds(currentViewerPos + topLeftViewCornerDisplacement + _bufferViewDist * topLeftViewCornerDisplacement.normalized);
        Vector2Int bottomRightChunkCoord = ConvertToChunkPositionInBounds(currentViewerPos - topLeftViewCornerDisplacement + _bufferViewDist * (-topLeftViewCornerDisplacement.normalized));

        for (int t = topLeftChunkCoord.y; t >= bottomRightChunkCoord.y; t--)
        {
            for (int s = topLeftChunkCoord.x; s <= bottomRightChunkCoord.x; s++)
            {
                if (_chunks.Get(s, t) == null)
                {
                    var x = ToWorldPosition(s);
                    var y = ToWorldPosition(t);

                    Vector2 centrePositionWorldSpace = ChunkSize * new Vector2(x + ToWorldPosition(0.5f), y + ToWorldPosition(0.5f));
                    
                    SpatialArray<Tiles>[] chunkTileLayers = new SpatialArray<Tiles>[GeneratorManager.NTileLayers];
                    for (int i = 0; i < GeneratorManager.NTileLayers; i++)
                    {
                        chunkTileLayers[i] = new SpatialArray<Tiles>(ChunkSize.x, ChunkSize.y);
                        SetChunkTileMap(chunkTileLayers[i], _worldGen.Layers[i], new Vector2Int(s, t));
                    }

                    Chunk chunk = new Chunk(chunkTileLayers, centrePositionWorldSpace, new Vector2Int(s, t), this.transform, _texGen);
                    //_texGen.AssignChunkTextureConjoined(chunkTileMap, chunk);
                    _chunks.Set(chunk, s, t);
                }

                _chunks.Get(s, t).SetAllVisible(true);
            }
        }


    }

    private void SetChunkTileMap(SpatialArray<Tiles> chunkTileMap, SpatialArray<Tiles> tileMap, Vector2Int chunkCoord)
    {
        Debug.Assert((chunkTileMap.Length == ChunkSize.x && chunkTileMap.Height == ChunkSize.y),
            "Incorrect tilemap dimensions");

        for (int j = 0; j < ChunkSize.y; j++)
        {
            for (int i = 0; i < ChunkSize.x; i++)
            {
                chunkTileMap.Set(tileMap.Get(chunkCoord.x * ChunkSize.x + i, chunkCoord.y * ChunkSize.y + j), i, j);
            }
        }
    }

    public void RegenerateChunkLayerAt(Vector2Int chunkCoord, GeneratorManager.TileLayerIndices layerIndex)
    {
        //Debug.Log($"coord: {chunkCoord}; is chunk null at coord: {_chunks.Get(chunkCoord.x, chunkCoord.y) == null}");
        Debug.Assert(_chunks.Get(chunkCoord.x, chunkCoord.y) != null, $"Chunk at {chunkCoord} is null!");

        ChunkLayer chunkLayer = _chunks.Get(chunkCoord.x, chunkCoord.y).ChunkLayers[(int)layerIndex];
        SetChunkTileMap(chunkLayer.ChunkTileMap, _worldGen.Layers[(int)layerIndex], chunkCoord);
        chunkLayer.RegenerateMesh();
    }

    //private void SetChunkTileMap(Tiles[] chunkTileMap, Tiles[] world, Vector2Int chunkCoord)
    //{
    //    // chunkCoord follows grid x,y conventions
    //    Debug.Assert((chunkTileMap.GetLength(0) == ChunkSize.x && chunkTileMap.GetLength(1) == ChunkSize.y),
    //        "Incorrect tilemap dimensions");

    //    // (s, t) live in grid-space
    //    int s = chunkCoord.x;
    //    int t = _worldGen.HeightInChunkUnits - 1 - chunkCoord.y;
    //    int start = (t * _worldGen.LengthInChunkUnits) * (ChunkSize.x * ChunkSize.y) + s * ChunkSize.x;  

    //    for (int j = 0; j < ChunkSize.y; j++)
    //    {
    //        for (int i = 0; i < ChunkSize.x; i++)
    //        {
    //            chunkTileMap[j * ChunkSize.x + i] = world[start + j * _worldGen.LengthInChunkUnits * ChunkSize.x + i];
    //        }
    //    }
    //}

    private void Update()
    {
        UpdateChunks();
    }

    private void OnValidate()
    {
        if (_bufferViewDist < 0)
        {
            _bufferViewDist = 0;
        }
    }
}
