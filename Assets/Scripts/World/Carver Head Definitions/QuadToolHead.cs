
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quad Carver Head", menuName = "Scriptable Objects/Carver Heads/Quad")]
public class QuadToolHead : CarverHead
{
    //public override float PrimarySize { get; set; }  // obsolete
    [field: SerializeField] public override int PrimarySize { get; set; }
    [field: SerializeField] public override int SecondarySize { get; set; }

    public bool XEven { get => PrimarySize % 2 == 0; }
    public bool YEven { get => SecondarySize % 2 == 0; }

    public override HashSet<(Vector2Int, Tiles)> Carve(Vector2 position, SpatialArray<Tiles> map, Tiles tileToReplaceWith, HashSet<Tiles> tileDomain, int limit = int.MaxValue)
    {
        Vector2 positionMap = ChunkManager.ToMapPosition(position);

        var affectedTiles = new HashSet<(Vector2Int, Tiles)>();

        var originatingPixel = ChunkManager.ToBlockPosition(positionMap);
        int xDistLeft, xDistRight;
        int yDistUp, yDistDown;

        if (XEven)
        {
            xDistLeft = Mathf.CeilToInt((PrimarySize - 1) / 2f);
            xDistRight = PrimarySize - 1 - xDistLeft;
        }
        else
        {
            xDistLeft = xDistRight = (PrimarySize - 1) / 2;
        }

        if (YEven)
        {
            yDistUp = Mathf.CeilToInt((SecondarySize - 1) / 2f);
            yDistDown = SecondarySize - 1 - yDistUp;
        }
        else
        {
            yDistUp = yDistDown = (SecondarySize - 1) / 2;
        }

        var tl = new Vector2Int(originatingPixel.x - xDistLeft, originatingPixel.y + yDistUp);
        var br = new Vector2Int(originatingPixel.x + xDistRight, originatingPixel.y - yDistDown);

        int nTilesCarved = 0;
        for (var y = tl.y; y >= br.y; y--)
        {
            for (var x = tl.x; x <= br.x; x++)
            {
                if (nTilesCarved >= limit)
                {
                    return affectedTiles;
                }

                if (!map.CheckInBounds(x, y))
                {
                    return affectedTiles;
                }

                if (tileDomain.Contains(map.Get(x, y)))
                {
                    affectedTiles.Add((new Vector2Int(x,y), map.Get(x, y)));
                    map.Set(tileToReplaceWith, x, y);
                    nTilesCarved++;
                }
            }
        }
        return affectedTiles;
    }

    private void OnValidate()
    {
        if (PrimarySize < 0)
        {
            PrimarySize = 0;
        }
        if (SecondarySize < 0)
        {
            SecondarySize = 0;
        }
    }
}
