using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileTextureGenerator : MonoBehaviour
{
    //[SerializeField] private TileManager _tileManager;
    [SerializeField] private Texture2D _tileAtlas;
    [SerializeField] private Texture2D _decorationAtlas;
    [SerializeField] private int _maxTileResolution;  // const

    [Range(0,3)]
    [SerializeField] private int _macroTextureSamplingLevel;
    private int _macroTextureSampleSize = 1;

    private int _nTilesPerRow;
    private int _nRows;
    private Color[][] _tiles;

    private void Awake()
    {
        for (var i = 0; i < _macroTextureSamplingLevel; i++)
        {
            _macroTextureSampleSize *= 2;
        }

        _nTilesPerRow = _tileAtlas.width / _maxTileResolution;
        _nRows = _tileAtlas.height / _maxTileResolution;
        _tiles = new Color[_nTilesPerRow * _nRows][];

        for (int j = 0; j < _nRows; j++)
        {
            for (int i = 0; i < _nTilesPerRow; i++)
            {
                _tiles[j * _nTilesPerRow + i] = _tileAtlas.GetPixels(i * _maxTileResolution, j * _maxTileResolution, _macroTextureSampleSize, _macroTextureSampleSize);
            }
        }
    }

    // TODO
    public void AssignChunkTextureDisjoint(Tiles[] chunkTileMap, MeshData meshData)
    {
        for (int j = 0; j < ChunkManager.ChunkSize.y; j++)
        {
            for (int i = 0; i < ChunkManager.ChunkSize.x; i++)
            {
                
            }
        }

    }

    // pixel based texture
    public Texture2D GenerateChunkTextureConjoined(SpatialArray<Tiles> chunkTileMap)
    {
        Color[] pixelColours = new Color[ChunkManager.ChunkSize.x * ChunkManager.ChunkSize.y];
        Texture2D texture = new Texture2D(ChunkManager.ChunkSize.x, ChunkManager.ChunkSize.y);
        //Debug.Log($"{chunkTileMap.Height}, {chunkTileMap.Length}");
        for (var y = 0; y < chunkTileMap.Height; y++)
        {
            for (var x = 0; x < chunkTileMap.Length; x++)
            {
                // Assumption: tile id matches with id in tile atlas (grid-space ordering)
                if (chunkTileMap.Get(x, y) != 0)
                {
                    pixelColours[y * ChunkManager.ChunkSize.x + x] = _tiles[(int)chunkTileMap.Get(x, y)][(y % _macroTextureSampleSize) * _macroTextureSampleSize + (x % _macroTextureSampleSize)];
                }
            }
        }
        texture.SetPixels(pixelColours);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        return texture;

    }

    // Tile based texture
    //public Texture2D GenerateChunkTextureConjoined(SpatialArray<Tiles> chunkTileMap)
    //{
    //    Texture2D texture = new Texture2D(ChunkManager.ChunkSize.x * _maxTileResolution, ChunkManager.ChunkSize.y * _maxTileResolution);

    //    for (int y = 0; y < chunkTileMap.Height; y++)
    //    {
    //        for (int x = 0; x < chunkTileMap.Length; x++)
    //        {
    //            // Assumption: tile id matches with id in tile atlas (grid-space ordering)
    //            if (chunkTileMap.Get(x, y) != 0)
    //            {
    //                texture.SetPixels(x * _maxTileResolution, y * _maxTileResolution, _maxTileResolution, _maxTileResolution, _tiles[(int)chunkTileMap.Get(x, y)]);
    //            }
    //        } 
    //    }

    //    texture.filterMode = FilterMode.Point;
    //    texture.wrapMode = TextureWrapMode.Clamp;
    //    texture.Apply();

    //    return texture;

    //}

    

}
