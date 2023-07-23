using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Component", menuName = "Scriptable Objects/Item Components/Weapon")]
public class WeaponComponent : ItemComponent, IEquatable<WeaponComponent>
{
    private WeaponComponent _originalData { get; set; }

    [field: SerializeField] public EntityAttackStats AttackStats { get; private set; }

    public override Item Item { get => _item;
        set
        {
            _item = value;
        }
    }

    //public WeaponComponent(WeaponComponentData data)
    //{
    //    OriginalData = data;
    //    AttackStats = (EntityAttackStats)OriginalData.AttackStats.Clone();
    //}

    public override object Clone()
    {
        var newComponent = ScriptableObject.CreateInstance<WeaponComponent>();
        newComponent._originalData = this;
        newComponent.AttackStats = (EntityAttackStats)AttackStats.Clone();
        return newComponent;
    }

    public override bool Equals(object other) => this.Equals(other as WeaponComponent);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(AttackStats.GetHashCode());
        return hash.ToHashCode();
    }

    public bool Equals(WeaponComponent other)
    {
        if (other == null)
        {
            return false;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        return AttackStats == other.AttackStats; 
    }

    public static bool operator ==(WeaponComponent lhs, WeaponComponent rhs)
    {
        if (object.ReferenceEquals(lhs, rhs))
        {
            return true;
        }
        if (object.ReferenceEquals(lhs, null))
        {
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(WeaponComponent lhs, WeaponComponent rhs) => !(lhs == rhs);
}
