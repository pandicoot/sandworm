using UnityEngine;

public class Chunk 
{
    public ChunkLayer[] ChunkLayers { get; private set; }
    public Vector2Int ChunkCoord { get; }
    private GameObject _chunkObj { get; }
    private Transform _chunkObjTransform { get; }

    //

    //MeshData _meshData;
    //SpatialArray<Tiles> _chunkTileMap;

    //GameObject _chunkObject;
    //Transform _chunkObjectTransform;
    //MeshFilter _meshFilter;
    //MeshRenderer _meshRenderer;

    //Texture2D _texture;

    //TextureGeneratorScriptableObject _textureGeneratorScriptableObject;

    //GameObject _chunkAnchor;

    public Chunk(SpatialArray<Tiles>[] layers, Vector3 centreWorldSpacePosition, Vector2Int chunkPosition, Transform parent, TileTextureGenerator texGen)
    {
        ChunkLayers = new ChunkLayer[GeneratorManager.NTileLayers];
        ChunkCoord = chunkPosition;
        _chunkObj = new GameObject(ChunkCoord.ToString());
        _chunkObjTransform = _chunkObj.transform;
        _chunkObjTransform.parent = parent;
        _chunkObjTransform.position = centreWorldSpacePosition;
        _chunkObj.SetActive(false);

        for (int i = 0; i < layers.GetLength(0); i++)
        {
            ChunkLayers[i] = new ChunkLayer(layers[i], _chunkObjTransform, i-1, texGen);
        }
    }

    //public Chunk(SpatialArray<Tiles> chunkTileMap, MeshData meshData, Vector3 centreWorldSpacePosition, Vector2Int chunkPosition, Transform parent, TextureGeneratorScriptableObject textureGeneratorScriptableObject)
    //{
    //    _meshData = meshData;
    //    _chunkTileMap = chunkTileMap;

    //    _chunkObject = new GameObject($"({chunkPosition.x}, {chunkPosition.y})");
        
    //    _meshFilter = _chunkObject.AddComponent<MeshFilter>();
    //    _meshRenderer = _chunkObject.AddComponent<MeshRenderer>();
    //    _chunkObjectTransform = _chunkObject.transform;
    //    _chunkObjectTransform.parent = parent;

    //    _chunkObjectTransform.position = centreWorldSpacePosition;
    //    _textureGeneratorScriptableObject = textureGeneratorScriptableObject;

    //    _meshFilter.mesh = _meshData.CreateMesh();

    //    _chunkObject.SetActive(false);


    //    // TODO: replace with gizmo
    //    _chunkAnchor = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //    _chunkAnchor.transform.position = centreWorldSpacePosition;
    //    _chunkAnchor.SetActive(false);
    //}

    //public void ApplyTexture(Texture2D texture)
    //{
    //    _texture = texture;
    //    _meshRenderer.material.mainTexture = _texture;
    //    _meshRenderer.material.shader = _textureGeneratorScriptableObject.TileShader;
    //}

    public void SetAllVisible(bool visible)
    {
        foreach (var l in ChunkLayers)
        {
            l.SetVisible(visible);
        }
        _chunkObj.SetActive(visible);
    }

    //public void SetVisible(bool visible)
    //{
    //    _chunkObject.SetActive(visible);
    //}

    //public void SetVisibleAnchor(bool visible)
    //{
    //    _chunkAnchor.SetActive(visible);
    //}
}
