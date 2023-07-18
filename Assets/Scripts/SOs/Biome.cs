using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Scriptable Objects/Biome")]
public class Biome : ScriptableObject
{
    public BiomeType Type;
    public float SurfaceHeightMultiplier;
    public AnimationCurve SurfaceHeightCurve;

    public Stratum[] Strata;  // list?
    public Stratum[] BackgroundStrata;
    public StratumInterface[] StratumInterfaces;
    public StratumInterface[] BackgroundStratumInterfaces;
    public NoiseParameters SurfaceNoiseParameters;
    
    public CaveBiome PrincipalCaveBiome;  // later replace with a list of Distributions, which include altitude data
    public CaveBiome AuxiliaryCaveBiome;

    public List<GameObject> Trees;
    public int TreeSpawnDepth;
    public float MaxRMSSteepnessForTreeSpawns;
    public float TreeSpawnProbability;
    public int MutualExclusionDistanceForTreeSpawns;

    public (Stratum, int) GetStratumAtDepth(int depth)
    {
        // depth == 0 indicates the topsoil layer
        int currDepth = 0;
        int i = -1;
        foreach (Stratum stratum in Strata)
        {
            i++;
            currDepth += stratum.Thickness;
            if (currDepth > depth)
            {
                return (stratum, i);  
            }
        }

        int nStrata = Strata.GetLength(0);
        return (Strata[nStrata], nStrata - 1);
    }
}

public enum BiomeType
{
    Forest
}

[System.Serializable]
public struct Stratum
{
    public Tiles Tile;
    public int Thickness;
}

//[Flags]
//public enum StratumInterfaceBlendingModes
//{
//    None = 0,
//    Dithering = 1,
//    Clumps = 2,
//    Cracks = 4
//}

[System.Serializable]
public struct StratumInterface
{
    public int IndexOfAboveStratum;

    public Tiles TileAbove;
    public Tiles TileBelow;

    public bool DoDither;
    public int DitheringWidth;
    public float DitherSize;
    public AnimationCurve DitheringCurve; // x=0.0: interfaceY + DitheringWidth, x=1.0: interfaceY - DitheringWidth; y value represents pr of selecting TileBelow, 1-y is pr of selecting TileAbove.
    //public StratumInterfaceBlendingModes BlendingMode;


}