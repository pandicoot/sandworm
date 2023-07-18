using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarve 
{
    public HashSet<(Vector2Int, Tiles)> Carve(Vector2 worldPosition, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain);
}
