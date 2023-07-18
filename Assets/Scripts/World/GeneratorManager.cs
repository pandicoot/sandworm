using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

public class GeneratorManager : MonoBehaviour
{
    public static Action<SpatialArray<Tiles>> WorldLoaded;
    public static Action<SpatialArray<Tiles>[]> AllLayersLoaded;

    

    public enum TileLayerIndices
    {
        ForegroundDeco,
        Principal,
        BackgroundDeco,
        Background
    }

    public const int NTileLayers = 4;
    public const int PrincipalLayerIndex = (int)TileLayerIndices.Principal;
    private List<IGenerator>[] Generators = new List<IGenerator>[NTileLayers];
    public SpatialArray<Tiles>[] Layers = new SpatialArray<Tiles>[NTileLayers];
    
    public SpatialArray<Tiles> World { get; private set; }
    public int[] SurfaceLevels { get; private set; }

    public Transform BackgroundObjects { get; private set; }

    private static int _lengthInChunkUnits = 40;
    public static int LengthInChunkUnits { get => _lengthInChunkUnits; }

    private static int _heightInChunkUnits = 20;
    public static int HeightInChunkUnits { get => _heightInChunkUnits; }

    [SerializeField] private int _seaLevel;
    public int SeaLevel { get => _seaLevel; }

    [SerializeField] private Biome _biome;
    public Biome Biome { get => _biome; }

    [SerializeField] private int _seed;
    [SerializeField] private NoiseParameters _stratumInterfaceNoiseParameters;
    [SerializeField] private float _stratumInterfaceThicknessMultiplier;

    [field: SerializeField] private float _carverSpawnMutualExclusionRadius;

    private void Awake()
    {
        //Debug.Log(transform.childCount);
        BackgroundObjects = transform.GetChild(0);
    }

    public void Generate()
    {
        // Initialise
        for (int i = 0; i < NTileLayers; i++)
        {
            Layers[i] = new SpatialArray<Tiles>(_lengthInChunkUnits * ChunkManager.ChunkSize.x, _heightInChunkUnits * ChunkManager.ChunkSize.y);
            Generators[i] = new List<IGenerator>();
        }


        World = Layers[PrincipalLayerIndex];
        SurfaceLevels = new int[_lengthInChunkUnits * ChunkManager.ChunkSize.x];

        GeneratePrincipalLayer();

        GenerateBackground();

        AllLayersLoaded?.Invoke(Layers);
    }

    private void GeneratePrincipalLayer()  // generalise
    {
        Generators[PrincipalLayerIndex].Add(new TopographyGenerator(_biome.SurfaceNoiseParameters, _biome.SurfaceHeightMultiplier, _biome.SurfaceHeightCurve, _seaLevel, _seed));
        Generators[PrincipalLayerIndex].Add(new StrataGenerator(_biome.Strata, _biome.StratumInterfaces, _stratumInterfaceNoiseParameters, _stratumInterfaceThicknessMultiplier));
        Generators[PrincipalLayerIndex].Add(new CaveGenerator(_biome.AuxiliaryCaveBiome, _carverSpawnMutualExclusionRadius));
        Generators[PrincipalLayerIndex].Add(new CaveGenerator(_biome.PrincipalCaveBiome, _carverSpawnMutualExclusionRadius));

        foreach (IGenerator g in Generators[PrincipalLayerIndex])
        {
            g.ApplyGenerator(World, SurfaceLevels);
        }

        WorldLoaded?.Invoke(World);
    }

    private void GenerateBackground()
    {
        Generators[(int)TileLayerIndices.Background].Add(new StrataGenerator(_biome.BackgroundStrata, _biome.BackgroundStratumInterfaces, _stratumInterfaceNoiseParameters, _stratumInterfaceThicknessMultiplier));
        Generators[(int)TileLayerIndices.Background].Add(new NaturalsGenerator(scanDist:3, _biome.MaxRMSSteepnessForTreeSpawns, _biome.TreeSpawnProbability, _biome.TreeSpawnDepth, Biome.MutualExclusionDistanceForTreeSpawns, _biome.Trees[0], BackgroundObjects));

        foreach (IGenerator g in Generators[(int)TileLayerIndices.Background])
        {
            g.ApplyGenerator(Layers[(int)TileLayerIndices.Background], SurfaceLevels);
        }
    }

    private void Update()
    {
        WorldLoaded?.Invoke(World);
        AllLayersLoaded?.Invoke(Layers);
    }

}
