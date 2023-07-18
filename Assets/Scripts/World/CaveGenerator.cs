using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : IGenerator
{
    [field: SerializeField] public float MutualExclusionRadius { get; private set; }
    private int _nCarversToTrySpawn;

    private List<(CarverWeightings, Vector2Int)> _carverOrigins;
    private List<Carver> _carvers;

    [field: SerializeField] public CaveBiome CaveBiome { get; private set; }

    public CaveGenerator(CaveBiome caveBiome, float mutualExclusionRadius)
    {
        CaveBiome = caveBiome;
        MutualExclusionRadius = mutualExclusionRadius;
    }

    private Vector2Int? TryGenerateCarverSpawnPosition(SpatialArray<Tiles> world, int[] surfaceLevels, CarverWeightings cw)
    {
        int x = Random.Range(0, world.Length);  // need to make these dependent on world seed
        int y = Mathf.RoundToInt(cw.SpawnDepthCurve.Evaluate(Random.value) * (surfaceLevels[x] + 1 - cw.SurfaceSpawnBuffer));

        foreach ((var cw_, var position) in _carverOrigins)
        {
            if (Vector2.SqrMagnitude(position - new Vector2Int(x, y)) < MutualExclusionRadius * MutualExclusionRadius)
            {
                return null;
            }
        }

        return new Vector2Int(x, y);
    }

    public void ApplyGenerator(SpatialArray<Tiles> world, int[] surfaceLevels)
    {
        _nCarversToTrySpawn = Mathf.RoundToInt(CaveBiome.CaveFrequency * world.Length * world.Height);
        //Debug.Log($"Number of carvers to spawn: {_nCarversToSpawn}");

        _carvers = new List<Carver>();
        _carverOrigins = new List<(CarverWeightings, Vector2Int)>();

        // get positions
        Vector2Int? pos;
        for (int i = 0; i < _nCarversToTrySpawn; i++)
        {
            var cw = SelectCarver(CaveBiome.CarverWeightings);
            pos = TryGenerateCarverSpawnPosition(world, surfaceLevels, cw);
            if (pos.HasValue)
            {
                _carverOrigins.Add((cw, pos.Value));
            }
        }

        // spawn
        foreach ((var cw, var position) in _carverOrigins)
        {
            //Debug.Log(position);
            //var cw = SelectCarver(CaveBiome.CarverWeightings);

            var carverHead = (CarverHead)cw.CarverHead.Clone();
            _carvers.Add(new Carver(world, surfaceLevels, CaveBiome.MeanCaveAmplitude, cw.Tile, position, cw.CarverParameters, carverHead, cw.CarverHeadParameters));
        }

        foreach (Carver carver in _carvers)
        {
            carver.Run();
        }

    }

    private CarverWeightings SelectCarver(List<CarverWeightings> carverWeightings)  // utility method? make generic 
    {
        float total = 0;
        for (int i = 0; i < carverWeightings.Count; i++)
        {
            total += carverWeightings[i].Weighting;
        }

        float pt = Random.value * total;

        for (int i = 0; i < carverWeightings.Count; i++)
        {
            if (pt < carverWeightings[i].Weighting)
            {
                return carverWeightings[i];
            }
            pt -= carverWeightings[i].Weighting;
        }

        return carverWeightings[carverWeightings.Count - 1];
    }
}
