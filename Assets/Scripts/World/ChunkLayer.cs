using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents a single chunk of a layer belonging to a particular world chunk
/// </summary>
public class ChunkLayer 
{
    public SpatialArray<Tiles> ChunkTileMap { get; set; }
    //public List<GameObject> DecorationPrefabs { get; private set; }

    private MeshData _meshData { get; set; }

    private GameObject _layerObj;
    private Transform _layerObjTransform;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Texture2D _texture;

    private TileTextureGenerator _texGen;

    public ChunkLayer(SpatialArray<Tiles> chunkTileMap, Transform parent, int zOffset, TileTextureGenerator texGen)
    {
        _layerObj = new GameObject();
        _meshFilter = _layerObj.AddComponent<MeshFilter>();
        _meshRenderer = _layerObj.AddComponent<MeshRenderer>();
        _layerObjTransform = _layerObj.transform;
        _layerObjTransform.parent = parent;
        _layerObjTransform.localPosition = Vector3.forward * zOffset;
        _layerObj.SetActive(false);

        _texGen = texGen;
        ChunkTileMap = chunkTileMap;
        RegenerateMesh();
    }

    public void RegenerateMesh()
    {
        _meshData = TileMeshGenerator.GenerateTileMesh(ChunkTileMap);
        _meshFilter.mesh = _meshData.CreateMesh();
        ApplyTexture(_texGen.GenerateChunkTextureConjoined(ChunkTileMap));
    }

    public void ApplyTexture(Texture2D texture)
    {
        _texture = texture;
        _meshRenderer.material.mainTexture = _texture;
        _meshRenderer.material.shader = Shader.Find("Unlit/Texture");
    }

    public void SetVisible(bool visible)
    {
        _layerObj.SetActive(visible);
    }
}
