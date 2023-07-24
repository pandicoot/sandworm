using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Radial Carver Head", menuName = "Scriptable Objects/Carver Heads/Radial")]
public class RadialToolHead : CarverHead
{
    //[field: SerializeField] public override float PrimarySize { get; set; }  // in map units
    //[SerializeField] private int _radius;
    [field: SerializeField] public override int PrimarySize { get; set; }
    public override int SecondarySize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //public override float PrimarySize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //public override float SecondarySize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //[field: SerializeField] public bool IsEven { get => Radius % 2 == 0; }

    public override HashSet<(Vector2Int, Tiles)> Carve(Vector2 position, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain, int limit = int.MaxValue)
    {
        Vector2 positionMap = ChunkManager.ToMapPosition(position);

        var affectedTiles = new HashSet<(Vector2Int, Tiles)>();  // TODO: get caller to pass in

        var originPixel = ChunkManager.ToBlockPosition(positionMap);
        var tl = originPixel + new Vector2Int(-(PrimarySize - 1), PrimarySize - 1);
        var br = originPixel + new Vector2Int(PrimarySize - 1, -(PrimarySize - 1));

        int nTilesCarved = 0;
        for (var y = tl.y; y >= br.y; y--)
        {
            for (var x = tl.x; x <= br.x; x++)
            {
                if (nTilesCarved >= limit)
                {
                    return affectedTiles;
                }

                if (Vector2.SqrMagnitude(new Vector2(x, y) - originPixel) > PrimarySize * PrimarySize)
                {
                    continue;
                }

                if (!map.CheckInBounds(x, y))
                {
                    return affectedTiles;
                }

                if (tileDomain.Contains(map.Get(x, y)))
                {
                    affectedTiles.Add((new Vector2Int(x ,y), map.Get(x, y)));
                    map.Set(tileToReplaceWith, x, y);
                    nTilesCarved++;
                }
            }
        }
        return affectedTiles;
    }

    private void OnValidate()
    {
        if (PrimarySize < 1)
        {
            PrimarySize = 1;
        }
    }
}

