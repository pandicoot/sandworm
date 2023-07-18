using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemEntity))]
public class ItemAggregation : MonoBehaviour
{
    public static readonly float AggregationRadius = 12f;

    private ItemEntity _itemEntity { get; set; }

    private void Awake()
    {
        _itemEntity = GetComponent<ItemEntity>();
    }

    void TryAggregateToNearbyEntities()  // probably not optimal, could do a DP solution lol?
    {
        var spawnedItemEntities = _itemEntity.ItemManager.SpawnedItemEntities.ToArray();

        // biggest-first
        var possibleAggregands = Array.FindAll<ItemEntity>(spawnedItemEntities, e =>
            Vector2.SqrMagnitude(e.transform.position - transform.position) < AggregationRadius * AggregationRadius &&
            e.ItemStack.Item == this._itemEntity.ItemStack.Item &&
            e.ItemStack.Count >= this._itemEntity.ItemStack.Count
        );

        var bestItemEntityOfSameType = this._itemEntity;

        foreach (ItemEntity itemEntity in possibleAggregands)
        {
            if (itemEntity.ItemStack.Count < itemEntity.ItemStack.Capacity &&
                itemEntity.ItemStack.Count >= bestItemEntityOfSameType.ItemStack.Count)
            {
                bestItemEntityOfSameType = itemEntity;
            }
        }

        if (bestItemEntityOfSameType == this._itemEntity)
        {
            return;
        }

        // aggregate
        this._itemEntity.ItemStack = bestItemEntityOfSameType.ItemStack.TryAdd(this._itemEntity.ItemStack, out int numAdded);
        if (this._itemEntity.ItemStack.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TryAggregateToNearbyEntities();
    }

    void Update()
    {
        TryAggregateToNearbyEntities();

    }
}
