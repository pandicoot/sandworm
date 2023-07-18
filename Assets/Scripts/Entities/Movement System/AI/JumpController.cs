using UnityEngine;

public class JumpController 
{
    public JumpController(SpatialArray<Tiles> map, Vector2 extents, Transform transform)
    {
        Map = map;
        Extents = extents;
        Transform = transform;
    }

    public SpatialArray<Tiles> Map { get; set; }
    public Vector2 Extents { get; set; }
    public Transform Transform { get; set; }

    public void UpdateJump(ref bool jumpInput, Vector2 direction)
    {
        var feetPosMap = ChunkManager.ToBlockPosition(ChunkManager.ToMapPosition(new Vector2(Transform.position.x + (direction.x > 0 ? Extents.x : -Extents.x), Transform.position.y - Extents.y)));
        var aheadPosMap = feetPosMap + (direction.x > 0 ? Vector2Int.right : Vector2Int.left);
        if (Map.Get(aheadPosMap.x, aheadPosMap.y) != Tiles.Air)
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }

    }

    //public void UpdateJump(ref bool jumpInput, Vector2 direction)
    //{
    //    var feetBlockPos = new Vector2Int(Mathf.RoundToInt(Transform.position.x), Mathf.RoundToInt(Transform.position.y - Extents.y));
    //    var aheadPos = feetBlockPos + (direction.x > 0 ? Vector2Int.right : Vector2Int.left);
    //    if (Map.Get(aheadPos.x, aheadPos.y) != Tiles.Air)
    //    {
    //        jumpInput = true;
    //    }
    //    else
    //    {
    //        jumpInput = false;
    //    }

    //}

}