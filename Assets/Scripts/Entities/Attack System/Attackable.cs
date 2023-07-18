using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Hitbox), typeof(Health))]
public class Attackable : MonoBehaviour
{
    public Health Health { get; private set; }

    public static float InvincibilityFrameDurationSeconds = 0.2f;

    public Action<Attack> OnAttacked { get; private set; }

    public Transform Transform { get; private set; }
    private MovementController _movementController { get; set; }
    public CharacterMovement Movement { get; private set; }

    private Vector2 Size { get; set; }
    public Bounds Bounds { get => new Bounds(Transform.position, Size); }

    private SpriteRenderer _sr;  // temp
    private Color _ogColour;

    //[SerializeField] private int _hitPoints;  // serialize to be read only in Inspector
    //public int HitPoints
    //{
    //    get => _hitPoints;
    //    set
    //    {
    //        _hitPoints = (value > 0) ? Mathf.Min(value, HitPointCapacity) : 0;
    //    }
    //}
    //[field: SerializeField] public int HitPointCapacity { get; private set; }

    //public bool IsAlive { get => HitPoints > 0; }

    //public bool IsInvincible { get; private set; }

    private void Awake()
    {
        Transform = transform;
        //IsInvincible = false;
        Health = GetComponent<Health>();
        _sr = GetComponent<SpriteRenderer>();
        Movement = GetComponent<CharacterMovement>();
        _movementController = GetComponent<MovementController>();
    }

    void Start()
    {
        //HitPoints = HitPointCapacity;
        Size = GetComponent<Hitbox>().Size;
        _ogColour = _sr.color;
    }

    private IEnumerator DoInvincibilityFrames(float duration)
    {
        yield return new WaitForSeconds(duration);
        Health.IsInvincible = false;
    }
    private IEnumerator DoColourChange(Color colour, float duration)
    {
        _sr.color = colour;
        yield return new WaitForSeconds(duration);
        _sr.color = _ogColour;    
    }

    //private void Damage(int damage)
    //{
    //    HitPoints -= damage;
    //}

    // temp
    public void ChangeColour(Color colour, float duration)
    {
        StartCoroutine(DoColourChange(colour, duration));
    }

    public virtual void TakeAttack(Attack attack)
    {
        if (!Health.IsInvincible)
        {
            //Debug.Log("Got attacked!");

            Health.Damage(attack.Damage);

            
            if (_movementController != null)
            {
                var knockbackDirection = attack.Direction - Movement.Velocity.normalized;
                _movementController.AddImpulse(knockbackDirection * attack.Knockback + Vector2.up * attack.Knockback / 2.5f);
            }

            ChangeColour(Color.red, 0.2f);

            Health.IsInvincible = true;
            StartCoroutine(DoInvincibilityFrames(InvincibilityFrameDurationSeconds));

            OnAttacked?.Invoke(attack);
        }
    }
}
