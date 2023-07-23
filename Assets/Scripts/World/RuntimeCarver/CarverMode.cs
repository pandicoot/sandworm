using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarverMode : State, ICarve
{
    [field: SerializeField] public float Size { get; private set; }
    [field: SerializeField] public float Inertia { get; private set; }
    // make Carver ai part of this?

    public abstract HashSet<(Vector2Int, Tiles)> Carve(Vector2 worldPosition, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain, int limit = int.MaxValue);
}
