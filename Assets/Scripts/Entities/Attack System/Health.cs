using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _hitPoints;  // serialize to be read only in Inspector
    public int HitPoints
    {
        get => _hitPoints;
        set
        {
            _hitPoints = (value > 0) ? Mathf.Min(value, HitPointCapacity) : 0;
        }
    }
    [field: SerializeField] public int HitPointCapacity { get; private set; }

    public bool IsAlive { get => HitPoints > 0; }
    private bool _dead = false;
    public bool IsInvincible { get; set; } = false;

    public event Action OnDied;

    private void Awake()
    {
        HitPoints = HitPointCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsAlive && !_dead)
        {
            OnDied?.Invoke();
            _dead = true;
        }
    }

    public void Damage(int damage)
    {
        HitPoints -= damage;
    }
}
