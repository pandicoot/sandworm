using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    //protected ItemManager ItemManager { get; set; }

    public ItemStack[] Slots { get; private set; }
    public int InventorySize { get; protected set; }

    public int IndexOfSelectedItem { get; private set; }

    //private Item _prevSelected { get; set; }
    //public event Action<Item> ChangedSelectedItem;

    //public event Action<int, int> MovedItems;

    public Inventory(int inventorySize)
    {
        InventorySize = inventorySize;
        Slots = new ItemStack[InventorySize];
        for (int i = 0; i < InventorySize; i++)
        {
            Slots[i] = new ItemStack();
        }
        IndexOfSelectedItem = 0;
        SubscribeToItemEvents(Slots[IndexOfSelectedItem].Item);
    }

    private void SubscribeToItemEvents(Item item)
    {
        if (item)
        {
            item.RequestRemove += OnConsumeSelected;
        }
    }
    private void UnsubscribeFromItemEvents(Item old)
    {
        if (old)
        {
            old.RequestRemove -= OnConsumeSelected;
        }
    }

    public void Select(int itemIdx)
    {
        if (itemIdx < 0 || itemIdx >= InventorySize)
        {
            throw new IndexOutOfRangeException("Item idx out of range");
        }

        UnsubscribeFromItemEvents(Slots[IndexOfSelectedItem].Item);
        IndexOfSelectedItem = itemIdx;
        SubscribeToItemEvents(Slots[itemIdx].Item);
        //Debug.Log(Slots[IndexOfSelectedItem].Item.RequestRemove);
        //Debug.Log(Slots[IndexOfSelectedItem].Item.GetInstanceID());
        //Debug.Log(Slots[IndexOfSelectedItem].Item.GetInvocations());
    }

    public void Select(Item item)
    {
        var idx = Array.FindIndex<ItemStack>(Slots, stack => stack.Item == item);
        if (idx < 0)
        {
            return;
        }

        UnsubscribeFromItemEvents(Slots[IndexOfSelectedItem].Item);
        IndexOfSelectedItem = idx;
        SubscribeToItemEvents(Slots[idx].Item);
    }

    public void HoldNextItem()
    {
        Select(IndexOfSelectedItem + 1);
    }

    public void HoldPrevItem()
    {
        Select(IndexOfSelectedItem - 1);
    }

    // Adds as many items from itemStack as is possible and returns the remaining itemStack.
    public ItemStack AddItem(ItemStack itemStack)
    {
        Debug.Assert(itemStack != null, "itemStack is null!");
        int numAdded;
        var itemStackAdd = itemStack;
        // add to stack check
        foreach (ItemStack slot in Slots)
        {
            if (itemStackAdd.Count == 0)
            {
                break;
            }
            itemStackAdd = slot.TryAdd(itemStackAdd, out numAdded);
        }
        return itemStackAdd;
    }

    //public void UpdateInventory()
    //{
    //    Selected = Slots[IndexOfSelectedItem].Item;
    //}

    //public void SwapStacks(int idx, ref ItemStack stack)
    //{
    //    var temp = Slots[idx];
    //    Slots[idx] = stack;
    //    stack = temp;
    //    //holder.Remove(stack.Count);
    //    //holder.Add(temp, out int numAdded);
    //}

    public ItemStack RemoveItem(int idx, int number)
    {
        if (idx < 0 || idx >= Slots.Length)
        {
            throw new IndexOutOfRangeException();
        }

       return Slots[idx].TryRemove(number);
    }

    public ItemStack RemoveStack(int idx)
    {
        if (idx < 0 || idx >= Slots.Length)
        {
            throw new IndexOutOfRangeException();
        }

        return Slots[idx].RemoveAll();
    }

    public (Item, int) GetSelected()
    {
        return (Slots[IndexOfSelectedItem].Item, Slots[IndexOfSelectedItem].Count);
    }

    private void OnConsumeSelected(int numToremove)
    {
        //Debug.Log($"trying to consume {numToremove} of {Slots[IndexOfSelectedItem].Item}");
        RemoveItem(IndexOfSelectedItem, numToremove);
    }

    //public void OnAttackWith(Attack attack)
    //{
    //    var item = Slots[IndexOfSelectedItem].Item;
    //    if (item)
    //    {
    //        item.OnAttackWith?.Invoke(attack);
    //    }
    //}

    //public void SwapItem(int idxFrom, int idxTo)
    //{
    //    if ((idxFrom < 0 || idxFrom >= Slots.Length) || (idxTo < 0 || idxTo >= Slots.Length))
    //    {
    //        throw new IndexOutOfRangeException();
    //    }

    //    if (Slots[idxFrom].Item == null)
    //    {
    //        return;
    //    }

    //    var temp = Slots[idxTo];
    //    Slots[idxTo] = Slots[idxFrom];
    //    Slots[idxFrom] = temp;

    //    //MovedItems?.Invoke(idxFrom, idxTo);
    //}

    //public void DropItem(Item item)
    //{
    //    var index = Array.IndexOf<Item>(Slots, item);
    //    if (index < 0)
    //    {
    //        return;
    //    }

    //    Slots[index] = null;
    //    // drop item (create new item representative gameobject in world, give item back to free item manager)
    //    //item.OnDrop();
    //}

    //public void DropItem(int index)
    //{
    //    if (index < 0 || index > Slots.Length - 1)
    //    {
    //        return;
    //    }
    //    var item = Slots[index];
    //    if (item == null)
    //    {
    //        return;
    //    }

    //    Slots[index] = null;
    //    //item.OnDrop();
    //}
}
