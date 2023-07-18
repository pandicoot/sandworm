using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(WorldGeneratorManagerPrototype))]
public class EditorWorldGeneratorManagerPrototype : Editor
{
    public override void OnInspectorGUI()
    {
        WorldGeneratorManagerPrototype manager = (WorldGeneratorManagerPrototype)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            manager.GenerateWorld();
        }
    }
    
}
