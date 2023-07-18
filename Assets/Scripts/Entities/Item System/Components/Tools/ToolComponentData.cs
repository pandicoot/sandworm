using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Tool Component", menuName = "Scriptable Objects/Item Components/Tool")]
public class ToolComponentData : ItemComponentData
{
    [field: SerializeField] public CarverHead Tool { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }

    public override ItemComponent InstantiateComponent()
    {
        throw new System.NotImplementedException();
        //return new ToolComponent(this);
    }
}
