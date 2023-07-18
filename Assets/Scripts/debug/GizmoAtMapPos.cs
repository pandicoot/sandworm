using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoAtMapPos 
{
    private static void DrawSquare(Vector2 pos, Vector2 oneOne, Color color)
    {
        Debug.DrawLine(pos, pos + oneOne, color, 0.4f);
        Debug.DrawLine(pos + new Vector2(oneOne.x, 0), pos + new Vector2(0, oneOne.y), color, 0.4f);
        Debug.DrawLine(pos, pos + new Vector2(0, oneOne.y), color, 0.4f);
        Debug.DrawLine(pos + new Vector2(0, oneOne.y), pos + oneOne, color, 0.4f);
        Debug.DrawLine(pos + oneOne, pos + new Vector2(oneOne.x, 0), color, 0.4f);
        Debug.DrawLine(pos + new Vector2(oneOne.x, 0), pos, color, 0.4f);
    }

    public static void DrawGizmoAt(Vector2 worldPosition, bool dispRawPos=true)
    {
        var mapPos = ChunkManager.ToMapPosition(worldPosition);

        var roundedPos = ChunkManager.ToWorldPosition(new Vector2(Mathf.RoundToInt(mapPos.x), Mathf.RoundToInt(mapPos.y)));
        var flooredPos = ChunkManager.ToWorldPosition(new Vector2(Mathf.FloorToInt(mapPos.x), Mathf.FloorToInt(mapPos.y)));
        var oneVector = ChunkManager.ToWorldPosition(Vector2.one);

        DrawSquare(roundedPos, oneVector, Color.white);
        DrawSquare(flooredPos, oneVector, Color.yellow);
        

        if (dispRawPos)
        {
            DrawSquare(worldPosition, 0.125f*oneVector, Color.white);
        }
    }
}
