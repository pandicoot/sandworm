using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise1D
{

    public static float[] GenerateNoiseMap(int length, float scale, int octaves, Vector2[] octaveCentres, float persistence, float lacunarity, bool zeroCentred = false)
    {
        // TODO: implement scrolling into/out of the page using manualSampleY
        
        if (length < 0 || octaves < 1 || octaveCentres == null)
        {
            return null;
        } 

        float[] noiseMap = new float[length];

        float noiseMin = float.MaxValue;
        float noiseMax = float.MinValue;

        for (int x = 0; x < length; x++)
        {
            float noiseHeight = 0;
            float octaveFrequency = 1;
            float octaveAmplitude = 1;

            for (int octaveIndex = 0; octaveIndex < octaves; octaveIndex++)
            {
                float sampleX = octaveFrequency * (x / scale + octaveCentres[octaveIndex].x);
                float sampleY = octaveCentres[octaveIndex].y;  // do I need to scale this by octave frequency? I don't think so?

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);;
                noiseHeight += perlinValue * octaveAmplitude;

                octaveFrequency *= lacunarity;
                octaveAmplitude *= persistence;
            }

            noiseMap[x] = noiseHeight;

            noiseMin = (noiseHeight < noiseMin) ? noiseHeight : noiseMin;
            noiseMax = (noiseHeight > noiseMax) ? noiseHeight : noiseMax;
        }

        // normalise
        for (int x = 0; x < length; x++)
        {
            if (zeroCentred)
            {
                noiseMap[x] = Mathf.InverseLerp(noiseMin, noiseMax, noiseMap[x]) * 2 - 1;
            }
            else
            {
                noiseMap[x] = Mathf.InverseLerp(noiseMin, noiseMax, noiseMap[x]);
            }
        }

        return noiseMap;
    }
}
