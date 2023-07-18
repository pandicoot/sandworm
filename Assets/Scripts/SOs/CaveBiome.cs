using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Cave Biome", menuName = "Scriptable Objects/Cave Biome")]
public class CaveBiome : ScriptableObject
{
    public float CaveFrequency;  // number of carvers to spawn per unit area
    public float MeanCaveAmplitude;  // biome-wide carver radius multiplier
    public List<CarverWeightings> CarverWeightings;
    
}

[System.Serializable]
public struct CarverWeightings
{
    public CarverParameters CarverParameters;
    public CarverHeadParameters CarverHeadParameters;  // can later merge these three, but might be better to keep separate
    public CarverHead CarverHead;
    public Tiles Tile;

    public int SurfaceSpawnBuffer;
    public AnimationCurve SpawnDepthCurve;
    public float Weighting;
}
