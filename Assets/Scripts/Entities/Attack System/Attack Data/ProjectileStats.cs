using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Stats", menuName = "Scriptable Objects/Attacks/Projectile")]
public class ProjectileStats : AttackStats, IEquatable<ProjectileStats>
{
    [field: SerializeField] public float BaseFireRate { get; private set; }
    [field: SerializeField] public float BaseImpulse { get; private set; }

    public override bool Equals(object other) => this.Equals(other as ProjectileStats);

    public bool Equals(ProjectileStats other)
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
            BaseFireRate == other.BaseFireRate &&
            BaseImpulse == other.BaseImpulse;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), BaseFireRate, BaseImpulse);
    }

    public static bool operator ==(ProjectileStats lhs, ProjectileStats rhs)
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

    public static bool operator !=(ProjectileStats lhs, ProjectileStats rhs) => !(lhs == rhs);

    public static ProjectileStats operator +(ProjectileStats a, ProjectileStats b)
    {
        var newStats = (ProjectileStats)((AttackStats)a + (AttackStats)b);
        newStats.BaseFireRate = a.BaseFireRate + b.BaseFireRate;
        newStats.BaseImpulse = a.BaseImpulse + b.BaseImpulse;
        return newStats;
    }
}
