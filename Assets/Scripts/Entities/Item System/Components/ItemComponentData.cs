using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemComponentData : ScriptableObject
{
    public abstract ItemComponent InstantiateComponent();
}
