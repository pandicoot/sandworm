using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCarverMode : CarverMode
{
    public override HashSet<(Vector2Int, Tiles)> Carve(Vector2 worldPosition, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain, int limit = int.MaxValue)
    {
        throw new System.NotImplementedException();
    }
}
