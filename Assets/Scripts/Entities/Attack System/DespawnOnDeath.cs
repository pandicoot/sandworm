using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeathBehaviour))]
public class DespawnOnDeath : MonoBehaviour, IOnDeath
{
    public void OnDeath()
    {
        Object.Destroy(gameObject);
    }

    
}
