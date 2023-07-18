using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(BlockGrid))]
public class BlockGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BlockGrid grid = (BlockGrid)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Clear grid"))
        {
            grid.DestroyAllBlocks();
        }
    }
}
