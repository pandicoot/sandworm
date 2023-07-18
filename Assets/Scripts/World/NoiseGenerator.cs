using UnityEngine;
using System;

public class NoiseGenerator
{
    // Each noise generator is for a particular seed.
    public NoiseParameters NoiseParams { get; set; }

    public int Seed { get; }
    public Vector2[] OctaveCentres { get; }

    private readonly System.Random _prng;

    // can also implement manual offset capability
    public NoiseGenerator(NoiseParameters noiseParams, int seed)
    {
        NoiseParams = noiseParams;

        Seed = seed;
        this._prng = new System.Random(Seed);
        Vector2[] octaveCentres = new Vector2[NoiseParams.Octaves];
        for (int i = 0; i < NoiseParams.Octaves; i++)
        {
            float offsetX = _prng.Next(-100000, 100000);
            float offsetY = _prng.Next(-100000, 100000);
            octaveCentres[i] = new Vector2(offsetX, offsetY);
        }

        OctaveCentres = octaveCentres;
    }

    public NoiseGenerator(NoiseParameters noiseParams)
    {
        NoiseParams = noiseParams;

        this._prng = new System.Random();
        Vector2[] octaveCentres = new Vector2[NoiseParams.Octaves];
        for (int i = 0; i < NoiseParams.Octaves; i++)
        {
            float offsetX = _prng.Next(-100000, 100000);
            float offsetY = _prng.Next(-100000, 100000);
            octaveCentres[i] = new Vector2(offsetX, offsetY);
        }

        OctaveCentres = octaveCentres;
    }

    public float[] Generate(int length, bool zeroCentred = false)
    {
        return Noise1D.GenerateNoiseMap(length, NoiseParams.Scale, NoiseParams.Octaves, OctaveCentres, NoiseParams.Persistence, NoiseParams.Lacunarity, zeroCentred); 
    }

}
