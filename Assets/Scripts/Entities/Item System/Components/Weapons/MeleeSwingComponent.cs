using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwingComponent : ItemComponent
{
    [field: SerializeField] public MeleeAttackStats MeleeAttackStats { get; private set; }

    private Item _item;
    public override Item Item { get => _item; set => _item = value; }
}
