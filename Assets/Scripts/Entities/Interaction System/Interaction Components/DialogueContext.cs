using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Context", menuName = "Scriptable Objects/Dialogue/Context")]
public class DialogueContext : ScriptableObject
{
    //public static DialogueContext TestContext = ScriptableObject.CreateInstance<DialogueContext>();

    [field: SerializeField] public bool TestField { get; set; }
}
