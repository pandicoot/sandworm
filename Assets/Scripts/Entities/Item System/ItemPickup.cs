using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventories))]
public class ItemPickup : MonoBehaviour
{
    [field: SerializeField] public ItemManager ItemManager { get; private set; }
    public Inventories Inventories { get; private set; }
    [field: SerializeField] public float ItemPickupRange { get; set; }

    private void Awake()
    {
        Inventories = GetComponent<Inventories>();
    }

    private void Start()
    {
        Debug.Assert(ItemManager, "ItemManager is null!");
    }

    void Update()
    {
        var spawnedItemEntities = ItemManager.SpawnedItemEntities.ToArray();

        //string s = "";
        //for (int i = 0; i < spawnedItemEntities.Length; i++)
        //{
        //    s += spawnedItemEntities[i] + ", ";
        //}
        //Debug.Log(s);

        for (int i = 0; i < spawnedItemEntities.Length; i++)  // n naive
        {
            var itemEntity = spawnedItemEntities[i];
            if (Vector2.SqrMagnitude(itemEntity.transform.position - transform.position) < ItemPickupRange * ItemPickupRange)
            {
                Inventories.TryPickup(itemEntity);
            }
        }
    }
}
