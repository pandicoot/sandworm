using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Weapon Component", menuName = "Scriptable Objects/Item Components/Weapon")]
public class WeaponComponentData : ItemComponentData
{
    [field: SerializeField] public EntityAttackStats AttackStats { get; private set; }

    public override ItemComponent InstantiateComponent()
    {
        throw new System.NotImplementedException();
        //return new WeaponComponent(this);
    }
}
