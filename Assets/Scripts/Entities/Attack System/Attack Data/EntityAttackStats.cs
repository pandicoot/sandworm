using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Attack Stats", menuName = "Scriptable Objects/Attacks/Entity Attack Stats")]
public class EntityAttackStats : AttackStats, IEquatable<EntityAttackStats>
{
    [field: SerializeField] public float BaseAttackRange { get; protected set; }
    [field: SerializeField] public float BaseAttackArcAngle { get; protected set; }
    [field: SerializeField] public float BaseAttackSpeed { get; protected set; }

    public override bool Equals(object other) => this.Equals(other as EntityAttackStats);

    public bool Equals(EntityAttackStats other)
    {
        if (other == null)
        {
            return false;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        return base.Equals(other) &&
            BaseAttackRange == other.BaseAttackRange &&
            BaseAttackArcAngle == other.BaseAttackArcAngle &&
            BaseAttackSpeed == other.BaseAttackSpeed;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(BaseAttackRange);
        hash.Add(BaseAttackArcAngle);
        hash.Add(BaseAttackSpeed);
        return hash.ToHashCode();
    }

    public static bool operator ==(EntityAttackStats lhs, EntityAttackStats rhs)
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

    public static bool operator !=(EntityAttackStats lhs, EntityAttackStats rhs) => !(lhs == rhs);

    public static EntityAttackStats operator +(EntityAttackStats a, EntityAttackStats b)
    {
        var newStats = (EntityAttackStats)((AttackStats)a + (AttackStats)b);
        //var newStats = ScriptableObject.CreateInstance<EntityAttackStats>();
        //newStats.BaseAttackDamage = a.BaseAttackDamage + b.BaseAttackDamage;
        //newStats.BaseKnockback = a.BaseKnockback + b.BaseKnockback;
        newStats.BaseAttackRange = a.BaseAttackRange + b.BaseAttackRange;
        newStats.BaseAttackArcAngle = a.BaseAttackArcAngle + b.BaseAttackArcAngle;
        newStats.BaseAttackSpeed = a.BaseAttackSpeed + b.BaseAttackSpeed;
        return newStats;
    }
}
