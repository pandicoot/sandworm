using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainMap : ScriptableObject
{
    public int Length { get => World?.Length ?? 0; }
    public int Height { get => World?.Height ?? 0; }

    public SpatialArray<Tiles>[] Layers = new SpatialArray<Tiles>[GeneratorManager.NTileLayers];

    public SpatialArray<Tiles> World { get; private set; }
}
