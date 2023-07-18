using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalsGenerator : IGenerator
{
    [field: SerializeField] public int ScanDist { get; private set; }
    //[field: SerializeField] public Biome Biome { get; private set; }
    public float MaxSteepness { get; private set; }
    public float SpawnProbability { get; private set; }
    public int SpawnDepth { get; private set; }
    public GameObject Prefab { get; private set; }
    public Transform Parent { get; private set; }
    public int MutualExclusionDistance { get; private set; }

    public NaturalsGenerator(int scanDist, float maxSteepness, float spawnProbability, int spawnDepth, int mutualExclusionDistance, GameObject prefab, Transform parent)
    {
        ScanDist = scanDist;
        MaxSteepness = maxSteepness;
        SpawnProbability = spawnProbability;
        Prefab = prefab;
        Parent = parent;
        SpawnDepth = spawnDepth;
        MutualExclusionDistance = mutualExclusionDistance;
    }

    private void GetGenerationSites(out List<int> positions, int[] surfaceLevels, float minSteepness, float maxSteepness, float probability)
    {
        positions = new List<int>();

        int length = surfaceLevels.Length;

        float minSteepnessSqr = minSteepness * minSteepness;
        float maxSteepnessSqr = maxSteepness * maxSteepness;

        int level;
        float meanSquareSteepness;
        int n;
        for (int x = 0; x < length; x++)
        {
            level = surfaceLevels[x];
            meanSquareSteepness = 0f;
            n = 0;
            for (int x_ = Mathf.Max(0, x - ScanDist); x_ < Mathf.Min(length, x + ScanDist); x_++)
            {
                meanSquareSteepness += (surfaceLevels[x_] - level) * (surfaceLevels[x_] - level);
                n++;
            }
            meanSquareSteepness /= n;

            //Debug.Log($"{meanSquareSteepness}, {minSteepnessSqr}, {maxSteepnessSqr}");

            if (meanSquareSteepness > minSteepnessSqr && meanSquareSteepness < maxSteepnessSqr)
            {
                if (Random.value <= probability && !positions.Exists(s => Mathf.Abs(s - x) < MutualExclusionDistance))
                {
                    positions.Add(x);
                }
            }
        }

    }

    public void ApplyGenerator(SpatialArray<Tiles> layer, int[] surfaceLevels)
    {
        GetGenerationSites(out List<int> positions, surfaceLevels, 0, MaxSteepness, SpawnProbability);
        
        foreach (var x in positions)
        {
            //Debug.Log($"Generate tree @ {x}");
            Object.Instantiate(Prefab, Parent.position + (Vector3)ChunkManager.ToWorldPosition(new Vector2(x, surfaceLevels[x] - SpawnDepth)), Quaternion.identity, Parent);
            
        }
        
    }
}
