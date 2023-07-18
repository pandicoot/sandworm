using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public static float ProjectileLifetimeSeconds = 30;

    public ProjectileStats ProjectileStats { get; set; }
    public PassiveAttacker PassiveAttacker { get; private set; }

    public GameObject Owner { get; set; }

    private void Awake()
    {
        Debug.Assert(TryGetComponent<EntityBase>(out EntityBase component), "Projectile is missing EntityBase component");
        PassiveAttacker = GetComponent<PassiveAttacker>();
    }

    private void Start()
    {
        if (PassiveAttacker)
        {
            if (Owner.TryGetComponent<Attackable>(out Attackable ownerAttackable))
            {
                PassiveAttacker.Friendlies.Add(ownerAttackable);
            }
        }

        StartCoroutine(DoLifetime(ProjectileLifetimeSeconds));
    }

    private void OnEnable()
    {
        if (PassiveAttacker) PassiveAttacker.OnCollide += OnImpact;
    }
    private void OnDisable()
    {
        if (PassiveAttacker) PassiveAttacker.OnCollide -= OnImpact;
    }

    private IEnumerator DoLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Object.Destroy(gameObject);  // TODO: release to pool
    }

    public void OnImpact()
    {
        Object.Destroy(gameObject);  // TODO: release to pool
    }
}
