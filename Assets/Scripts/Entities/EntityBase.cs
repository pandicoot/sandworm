using System;
using UnityEngine;

public class EntityBase : MonoBehaviour 
{
    [field: SerializeField] public EntityData EntityData { get; private set; }

    public event Action<GameObject> WasDestroyed;

    private void OnDestroy()
    {
        Debug.Log($"{this} in OnDestroy!");
        WasDestroyed?.Invoke(gameObject);
    }
}
