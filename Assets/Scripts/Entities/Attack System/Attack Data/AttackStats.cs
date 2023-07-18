using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Stats", menuName = "Scriptable Objects/Attacks/Attack Stats")]
public class AttackStats : Prototype, IEquatable<AttackStats>
{
    [field: SerializeField] public int BaseAttackDamage { get; protected set; }
    [field: SerializeField] public float BaseKnockback { get; protected set; }

    public override bool Equals(object other) => this.Equals(other as AttackStats);

    public bool Equals(AttackStats other)
    {
        if (other == null)
        {
            return false;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        return BaseAttackDamage == other.BaseAttackDamage && BaseKnockback == other.BaseKnockback;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(BaseAttackDamage);
        hash.Add(BaseKnockback);
        return hash.ToHashCode();
    }

    public static bool operator ==(AttackStats lhs, AttackStats rhs)
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

    public static bool operator !=(AttackStats lhs, AttackStats rhs) => !(lhs == rhs);

    public static AttackStats operator +(AttackStats a, AttackStats b)
    {
        var newStats = ScriptableObject.CreateInstance<EntityAttackStats>();
        newStats.BaseAttackDamage = a.BaseAttackDamage + b.BaseAttackDamage;
        newStats.BaseKnockback = a.BaseKnockback + b.BaseKnockback;
        return newStats;
    }
}
