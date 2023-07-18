using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DeathBehaviour : MonoBehaviour
{
    public Health Health { get; private set; }
    public List<IOnDeath> _deathBehaviours { get; private set; } = new List<IOnDeath>();

    private void Awake()
    {
        Health = GetComponent<Health>();
        _deathBehaviours.AddRange(GetComponents<IOnDeath>());
    }

    public void OnDeath()
    {
        _deathBehaviours.ForEach(behaviour => behaviour.OnDeath());
    }

    private void OnEnable()
    {
        Health.OnDied += OnDeath;
    }

    private void OnDisable()
    {
        Health.OnDied -= OnDeath;
    }
}
