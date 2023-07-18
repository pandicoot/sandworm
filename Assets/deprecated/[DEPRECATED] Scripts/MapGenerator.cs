using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [field: SerializeField] public int Length { get; private set; }
    [field: SerializeField] public float Scale { get; private set; }
    [field: SerializeField] public int Octaves { get; private set; }
    [field: SerializeField] public float Persistence { get; private set; }
    [field: SerializeField] public float Lacunarity { get; private set; }
    [field: SerializeField] public int Seed { get; private set; }

    [field: SerializeField] public int yMin { get; private set; }
    [field: SerializeField] public int SeaLevel { get; private set; }  
    [field: SerializeField] public float HeightMultiplier { get; private set; }
    [field: SerializeField] public AnimationCurve HeightCurve { get; private set; }

    private NoiseGenerator _noiseGen;
    [field: SerializeField] public bool AutoUpdate { get; private set; }
    [field: SerializeField] public BlockGrid Grid { get; private set; }

    //private bool _hasGeneratedUnderground;

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

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void AssignNoiseGenerator()
    {
        _noiseGen = new NoiseGenerator(PackageNoiseParameters());
        // TODO: only update noiseGen when SEED is changed. Otherwise can keep the same.
    }

    private void GenerateUnderground(GameObject block, int seaLevel)
    {
        for (int x = 0; x < Length; x++)
        {
            for (int y = seaLevel - 1; y >= yMin; y--)
            {
                Vector2Int blockCoords = new Vector2Int(x - Length / 2, y);
                Grid.PlaceBlock(blockCoords, block);
            }
        }

        //_hasGeneratedUnderground = true;  // just a workaround
    }

    public void GenerateMap()
    {
        AssignNoiseGenerator();
        Grid.DestroyAllBlocks();


        GameObject blockToSpawn = Grid.BlockInventory[0];  // temporary

        GenerateUnderground(blockToSpawn, SeaLevel);

        // TODO: BlockGrid class, adds safety to block placements.
        float[] heightMap = _noiseGen.Generate(Length);  // values are normalised from 0 to 1
        // normalise to [-1, 1] instead? Will have to change how the height curve is evaluated.

        for (int x = 0; x < Length; x++)
        {
            int surfaceLevel = SeaLevel + Mathf.RoundToInt(HeightMultiplier * HeightCurve.Evaluate(heightMap[x]));
            for (int y = surfaceLevel; y >= SeaLevel; y--)
            {
                Vector2Int blockCoords = new Vector2Int(x - Length / 2, y);
                Grid.PlaceBlock(blockCoords, blockToSpawn);
            }
        }
    }

}
