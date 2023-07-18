using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Texture2D Sprite { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string[] Tags { get; private set; }

    //[field: SerializeField] public ItemComponentData[] Components { get; private set; }

    [field: SerializeField] public WeaponComponentData WeaponComponent { get; private set; }
    [field: SerializeField] public ToolComponentData DestructComponent { get; private set; }
    [field: SerializeField] public ToolComponentData BuildComponent { get; private set; }

    //[field: SerializeField] public WeaponComponent[] WeaponComponents { get; private set; }
    //[field: SerializeField] public ToolComponent[] DestructComponents { get; private set; }
    //[field: SerializeField] public ToolComponent[] BuildComponents { get; private set; }

    public Item InstantiateItem()
    {
        throw new System.NotImplementedException();
        //return new Item(this);
    }
}
