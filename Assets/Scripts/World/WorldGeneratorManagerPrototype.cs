using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorManagerPrototype : MonoBehaviour
{
    [SerializeField] private TileMapGenerator _tileMapGenerator;
    [SerializeField] private TileMeshGenerator _tileMeshGenerator;
    private SpatialArray<Tiles> _world;
    private MeshData _tileMeshData;

    [field: SerializeField] public Vector2 Anchor { get; private set; }

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    //public bool autoUpdate;


    private void Start()
    {
        GenerateWorld();
    }

    public void DestroyExistingWorld()
    {
        _world = null;
        _tileMeshData = null;
        if (_meshFilter.sharedMesh != null)
        {
            DestroyImmediate(_meshFilter.sharedMesh);
        }
        _meshFilter.sharedMesh = null;
    }

    public void GenerateWorld()
    {
        DestroyExistingWorld();

        if (_tileMapGenerator != null)
        {
            _tileMapGenerator.GenerateMap();
            _world = _tileMapGenerator.World;
        }

        if (_tileMeshGenerator != null)
        {
            _tileMeshData = TileMeshGenerator.GenerateTileMesh(_world);
        }

        if (_meshFilter != null && _meshRenderer != null)
        {
            _meshFilter.sharedMesh = _tileMeshData.CreateMesh();
        }
    }
}
