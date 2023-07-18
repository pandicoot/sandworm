using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit Carver Head", menuName = "Scriptable Objects/Carver Heads/Unit")]
public class UnitToolHead : CarverHead
{
    public override int PrimarySize { get; set; } = 1;
    public override int SecondarySize { get; set; } = 1;

    public override HashSet<(Vector2Int, Tiles)> Carve(Vector2 position, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain)
    {
        Vector2 positionMap = ChunkManager.ToMapPosition(position);

        var affectedTiles = new HashSet<(Vector2Int, Tiles)>();

        var carvePos = ChunkManager.ToBlockPosition(positionMap);

        if (!map.CheckInBounds(carvePos))
        {
            return affectedTiles;
        }

        if (tileDomain.Contains(map.Get(carvePos.x, carvePos.y)))
        {
            affectedTiles.Add((carvePos, map.Get(carvePos.x, carvePos.y)));
            map.Set(tileToReplaceWith, carvePos.x, carvePos.y);
        }

        return affectedTiles;
    }
}
