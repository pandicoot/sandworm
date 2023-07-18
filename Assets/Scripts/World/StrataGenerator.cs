using System;
using UnityEngine;
public class StrataGenerator : IGenerator
{
    //[field: SerializeField] public Biome Biome { get; private set; }

    public NoiseParameters StratumInterfaceNoiseParameters { get; private set; }
    public float InterfaceThicknessMultiplier { get; private set; }

    public Stratum[] Strata { get; private set; }
    public StratumInterface[] StratumInterfaces { get; private set; }
    private NoiseGenerator _stratumInterfaceNoiseGen;

    public StrataGenerator(Stratum[] strata, StratumInterface[] stratumInterfaces, NoiseParameters noiseParams, float interfaceThicknessMultiplier)
    {
        Strata = strata;
        StratumInterfaces = stratumInterfaces;
        StratumInterfaceNoiseParameters = noiseParams;
        InterfaceThicknessMultiplier = interfaceThicknessMultiplier;
    }

    public void ApplyGenerator(SpatialArray<Tiles> world, int[] surfaceLevels)
    {
        var stratumYsBelowSurfaceLevel = new int[Strata.Length];

        var cumulativeDepth = 0;
        for (int i = 0; i < Strata.Length; i++)
        {
            cumulativeDepth += Strata[i].Thickness;
            stratumYsBelowSurfaceLevel[i] = cumulativeDepth - 1;
            // The expression surfaceLevels[x] - stratumYsBelowSurfaceLevel[i] yields the y value of the lowest block in that stratum, at that x.
        }

        //int[][] interfaceNoiseOffsets = new int[nStrata][];

        //interfaceNoiseOffsets[0] = new int[world.Length];
        //interfaceNoiseOffsets[nStrata - 1] = new int[world.Length];
        
        //for (int i = 1; i < nStrata - 1; i++)
        //{
        //    interfaceNoiseOffsets[i] = new int[world.Length];
        //    NoiseGenerator noiseGen = new NoiseGenerator(StratumInterfaceNoiseParameters);

        //    float[] noiseMap = noiseGen.Generate(world.Length, true);

        //    for (int x = 0; x < world.Length; x++)
        //    {
        //        interfaceNoiseOffsets[i][x] = Mathf.Clamp(Mathf.RoundToInt(InterfaceThicknessMultiplier * noiseMap[x]), -_strata[i].Thickness / 2, _strata[i].Thickness / 2);
        //    }
        //}

        // Generate noise offsets for each stratum interface
        int[][] stratumInterfaceNoiseOffsets = new int[StratumInterfaces.Length][];
        for (int i = 0; i < StratumInterfaces.Length; i++)
        {
            stratumInterfaceNoiseOffsets[i] = new int[world.Length];
            NoiseGenerator noiseGen = new NoiseGenerator(StratumInterfaceNoiseParameters);

            float[] noiseMap = noiseGen.Generate(world.Length, true);

            for (int x = 0; x < world.Length; x++)
            {
                stratumInterfaceNoiseOffsets[i][x] = Mathf.Clamp(Mathf.RoundToInt(InterfaceThicknessMultiplier * noiseMap[x]), -Strata[i].Thickness / 2, Strata[i].Thickness / 2);
            }
        }


        for (int x = 0; x < world.Length; x++)
        {
            SetTilesInColumn(world, x, surfaceLevels[x], stratumInterfaceNoiseOffsets);
        }

        // Stratum interface blending
        int[] interfaceYs = new int[world.Length];
        for (int i = 0; i < StratumInterfaces.Length; i++)
        {
            // assign appropriate interface Ys
            // TODO: move to top of function, use this for interface stuff
            Array.Clear(interfaceYs, 0, interfaceYs.Length);
            for (int x = 0; x < world.Length; x++)
            {
                interfaceYs[x] = surfaceLevels[x] - stratumYsBelowSurfaceLevel[StratumInterfaces[i].IndexOfAboveStratum] + stratumInterfaceNoiseOffsets[i][x];
            }

            BlendStratumInterface(world, interfaceYs, StratumInterfaces[i]);
        }

    }

    private void SetTilesInColumn(SpatialArray<Tiles> world, int x, int startingY, int[][] stratumInterfaceNoiseOffsets)
    {
        for (int i = 0; i < Strata.Length; i++)
        {
            int nStratumBlocksInColumn;
            if (i == Strata.Length - 1)
            {
                nStratumBlocksInColumn = Strata[i].Thickness;
            }
            else
            {
                var indexOfStratumInterface = Array.FindIndex<StratumInterface>(StratumInterfaces, sinterface => i == sinterface.IndexOfAboveStratum);
                //Debug.Log(indexOfStratumInterface);
                if (indexOfStratumInterface < 0)
                {
                    nStratumBlocksInColumn = Strata[i].Thickness;
                }
                else
                {
                    nStratumBlocksInColumn = Strata[i].Thickness + stratumInterfaceNoiseOffsets[indexOfStratumInterface][x];
                }
                
            }

            for (int j = 0; j < nStratumBlocksInColumn; j++)
            {
                world.Set(Strata[i].Tile, x, startingY--);
                if (startingY < 0)
                {
                    return;
                }
            }
        }

        // The lowest stratum is used for rest of the world.
        while (startingY >= 0)
        {
            world.Set(Strata[Strata.Length - 1].Tile, x, startingY--);
        }
    }

    private void BlendStratumInterface(SpatialArray<Tiles> world, int[] interfaceYs, StratumInterface stratumInterface)
    {
        for (int x = 0; x < world.Length; x++)
        {
            if (stratumInterface.DoDither)
            {
                Tiles tileToUse;
                for (int y = interfaceYs[x] + stratumInterface.DitheringWidth; y >= 0 && y >= interfaceYs[x] - stratumInterface.DitheringWidth; y--)
                {
                    if (UnityEngine.Random.value < stratumInterface.DitheringCurve.Evaluate((interfaceYs[x] + stratumInterface.DitheringWidth - y) / (float)(2 * stratumInterface.DitheringWidth + 1)))
                    {
                        tileToUse = stratumInterface.TileBelow;
                    }
                    else
                    {
                        tileToUse = stratumInterface.TileAbove;
                    }

                    world.Set(tileToUse, x, y);
                }
            }


        }
    }
}
