using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attack // struct instead?
{
    public Attack(Attackable attacker, Vector2 origin, Vector2 direction, float radius, float angle, int damage, float knockback)
    {
        Attacker = attacker;
        Origin = origin;
        Radius = radius;
        Angle = angle;
        Damage = damage;
        Knockback = knockback;
        Direction = direction;
    }

    public Attackable Attacker { get; }
    public Vector2 Origin { get; }
    public Vector2 Direction { get; }
    public float Radius { get; }
    public float Angle { get; }

    public int Damage { get; }
    public float Knockback { get; }

    public Bounds Bounds
    {
        get
        {
            return new Bounds(Origin + Direction * Radius / 2f, new Vector2(Radius, Radius));
        }
    }
}
