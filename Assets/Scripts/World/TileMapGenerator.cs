using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    private SpatialArray<Tiles> _world;

    public SpatialArray<Tiles> World
    {
        get => _world;
        private set => _world = value;
    }

    #region Map parameters
    [field: SerializeField] public Vector2Int Centre { get; private set; }
    [field: SerializeField] public int LengthInChunkUnits { get; private set; }   // in units of ChunkSize.x
    [field: SerializeField] public int HeightInChunkUnits { get; private set; }   // in units of ChunkSize.y
    [field: SerializeField] public int SeaLevel { get; private set; }

    [field: SerializeField] public float Scale { get; private set; }
    [field: SerializeField] public int Octaves { get; private set; }
    [field: SerializeField] public float Persistence { get; private set; }
    [field: SerializeField] public float Lacunarity { get; private set; }

    [field: SerializeField] public int Seed { get; private set; }

    [field: SerializeField] public float HeightMultiplier { get; private set; }
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; }
    #endregion

    private int _blockLength;  // make static
    private int _blockHeight;
    private NoiseGenerator _noiseGen;

    public struct TerrainType
    {
        Tiles tile;
        int depth;
        float spread;
    }


    public NoiseParameters PackageNoiseParameters()
    {
        return new NoiseParameters()
        {
            Scale = Scale,
            Octaves = Octaves,
            Persistence = Persistence,
            Lacunarity = Lacunarity,
        };
    }

    private void AssignNoiseGenerator()
    {
        _noiseGen = new NoiseGenerator(PackageNoiseParameters(), Seed);
    }

    private void GenerateUnderground(Tiles tile)
    {
        for (int x = 0; x < _blockLength; x++)
        {
            for (int y = 0; y < SeaLevel; y++)
            {
                _world.Set(tile, x, y);
            }
        }
    }

    public void GenerateMap()
    {
        _blockLength = LengthInChunkUnits * ChunkManager.ChunkSize.x;
        _blockHeight = HeightInChunkUnits * ChunkManager.ChunkSize.y;

        _world = new SpatialArray<Tiles>(_blockLength, _blockHeight);

        if (_noiseGen != null)
        {
            if (Seed != _noiseGen.Seed)
            {
                AssignNoiseGenerator();
            }
        }
        else
        {
            AssignNoiseGenerator();
        }

        Tiles topSoil = Tiles.Grass;
        Tiles underSoil = Tiles.Dirt;

        GenerateUnderground(Tiles.Stone);

        float[] heightMap = _noiseGen.Generate(_blockLength);  // values are normalised from 0 to 1
        // normalise to [-1, 1] instead? Will have to change how the height curve is evaluated.

        for (int x = 0; x < _blockLength; x++)
        {
            int surfaceLevel = Mathf.Min(_blockHeight, SeaLevel + Mathf.RoundToInt(HeightMultiplier * HeightCurve.Evaluate(heightMap[x])));
            _world.Set(topSoil, x, surfaceLevel);
            for (int y = SeaLevel; y < surfaceLevel; y++)
            {
                _world.Set(underSoil, x, y);
            }
        }

        Debug.Log(DisplayTilesAsText.DisplayAsText(_world, _blockLength, _blockHeight));
    }
}
