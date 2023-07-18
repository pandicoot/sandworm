using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarverHead : Prototype, ICarve
{
    public abstract int PrimarySize { get; set; }
    public abstract int SecondarySize { get; set; }

    public abstract HashSet<(Vector2Int, Tiles)> Carve(Vector2 worldPosition, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain);

    // Represents a function defined over the interval [-1, 1] 
    // with values ranging from 0 to 1. Returns true if a sample point
    // (sampleX, sampleY) is under the function over [-1, 1].
    //public abstract bool Function(float sampleX, float sampleY, float xMultiplier=1, float yMultiplier=1);
}
