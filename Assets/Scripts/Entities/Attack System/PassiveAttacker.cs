using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hitbox))]
public class PassiveAttacker : MonoBehaviour, IAttacker
{
    [field: SerializeField] public AttackStats AttackStats { get; private set; }

    public QuadtreeController QuadtreeController { get; set; }
    public Movement Movement { get; private set; }
    public Hitbox Hitbox { get; private set; }
    public Attackable Attackable { get; private set; }

    public List<Attackable> Friendlies { get; set; } = new List<Attackable>();

    private Vector2 _size { get => Hitbox.Size; }
    public Bounds Bounds { get => new Bounds(transform.position, _size); }
    public Attack Attack { get => new Attack(Attackable, transform.position, Vector2.zero, 0, 0, AttackStats.BaseAttackDamage, AttackStats.BaseKnockback); }

    private List<Attackable> _attackablesInRange = new List<Attackable>();

    private bool _collidedThisFrame { get; set; }
    public event Action OnCollide;

    private void Awake()
    {
        QuadtreeController = GetComponentInParent<QuadtreeController>();
        Movement = GetComponent<Movement>();
        Hitbox = GetComponent<Hitbox>();
        Attackable = GetComponent<Attackable>();
    }

    public bool TryAttack()
    {
        var didAttack = false;
        var attack = Attack;
        _attackablesInRange.Clear();
        QuadtreeController.GetComponentInBounds<Attackable>(Bounds, _attackablesInRange);

        foreach (Attackable attackable in _attackablesInRange)
        {
            if (attackable.gameObject == gameObject || Friendlies.Contains(attackable))
            {
                continue;
            }

            if (Bounds.Intersects(attackable.Bounds))
            {
                var direction = Vector2.zero;

                if (Movement != null)
                {
                    direction = Movement.Velocity.normalized;
                }
                attackable.TakeAttack(new Attack(attack.Attacker, attack.Origin, direction, 0f, 0f, attack.Damage, attack.Knockback));
                didAttack = true;
            }
        }
        return didAttack;
        
    }

    private void Update()
    {
        if (TryAttack())
        {
            OnCollide?.Invoke();
        }
    }

}
