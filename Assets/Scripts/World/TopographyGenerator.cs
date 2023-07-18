using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopographyGenerator : IGenerator
{
    [field: SerializeField] public int SeaLevel { get; private set; }
    [field: SerializeField] public int Seed { get; private set; }
    //[field: SerializeField] public Biome Biome { get; private set; }

    public NoiseParameters SurfaceNoiseParameters { get; private set; }
    public float SurfaceHeightMultiplier { get; private set; }
    public AnimationCurve SurfaceHeightCurve { get; private set; }

    private NoiseGenerator _noiseGen;

    public TopographyGenerator(NoiseParameters surfaceNoiseParameters, float surfaceHeightMultiplier, AnimationCurve surfaceHeightCurve, int seaLevel, int seed)
    {
        SeaLevel = seaLevel;
        Seed = seed;
        SurfaceNoiseParameters = surfaceNoiseParameters;
        SurfaceHeightMultiplier = surfaceHeightMultiplier;
        SurfaceHeightCurve = surfaceHeightCurve;
    }

    public void ApplyGenerator(SpatialArray<Tiles> world, int[] surfaceLevels)
    {
        if (_noiseGen == null)
        {
            _noiseGen = new NoiseGenerator(SurfaceNoiseParameters, Seed);
        }
        else
        {
            if (Seed != _noiseGen.Seed)
            {
                _noiseGen = new NoiseGenerator(SurfaceNoiseParameters, Seed);
            }
        }

        float[] heightMap = _noiseGen.Generate(world.Length);  // values are normalised from 0 to 1
        // normalise to [-1, 1] instead? Will have to change how the height curve is evaluated.

        for (int x = 0; x < world.Length; x++)
        {
            surfaceLevels[x] = Mathf.Min(world.Height, SeaLevel + Mathf.RoundToInt(SurfaceHeightMultiplier * SurfaceHeightCurve.Evaluate(heightMap[x])));
            for (int y = 0; y <= surfaceLevels[x]; y++)
            {
                world.Set((Tiles)1, x, y);
            }
        }

    }
}
