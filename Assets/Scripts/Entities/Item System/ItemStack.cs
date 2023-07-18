using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack 
{

    public const int DefaultStackCapacity = 81;
    public Item Item { get; private set; }

    public int Count { get; set; }
    private int _capacity;
    public int Capacity
    {
        get => _capacity;
        private set
        {
            Debug.Assert(value >= 0, "Capacity cannot be zero or negative");
            _capacity = value;
        }
    }
    public bool ShowStackIndicator { get => !(Count == 1 || Count == 0); }


    public ItemStack()
    {
    }

    public ItemStack(Item item, int count)
    {
        AcquireNew(item, count);
    }

    private void AcquireNew(Item item, int count)
    {
        //Debug.Assert(count > 0 && count <= item.StackLimit, "invalid count");

        if (item.StackLimit > 0)
        {
            Capacity = item.StackLimit;
        }
        else
        {
            Capacity = DefaultStackCapacity;
        }

        //ShowStackIndicator = item.StackLimit != 1;

        Item = item;
        Count = count;
    }

    //private void Acquire(Item item)
    //{
    //    if (Count != 0)
    //    {
    //        return;
    //    }

    //    if (item.StackLimit > 0)
    //    {
    //        Capacity = item.StackLimit;
    //    }
    //    else
    //    {
    //        Capacity = DefaultStackCapacity;
    //    }

    //    ShowStackIndicator = item.StackLimit != 1;

    //    Item = item;
    //    Count = 1;
    //}

    //public bool Add()  // take in itemstack instead?
    //{
    //    if (Count == 0 || Count == Capacity)
    //    {
    //        return false;
    //    }

    //    Count++;
    //    return true;
    //}

    //public int Add(int number)
    //{
    //    Debug.Assert(number >= 0, "Cannot add a negative amount");

    //    if (Count == 0 || number == 0)
    //    {
    //        return -1;
    //    }

    //    var numberToAdd = Mathf.Min(number, Capacity - Count);

    //    Count += numberToAdd;
    //    return numberToAdd;
    //}

    // Adds as many items from stack as it can and returns stack after the corresponding number of items have been removed.
    public ItemStack TryAdd(ItemStack stack, out int numAdded)
    {
        if (stack.Item == null)
        {
            numAdded = 0;
            return stack;
        }

        int num = 0;
        if (Item == null)
        {
            AcquireNew(stack.Item, stack.Count);
            num = Count;
            stack.TryRemove(Count);
        }
        else if (Item == stack.Item)  // compare item types
        {
            num = Mathf.Min(stack.Count, Capacity - Count);
            Count += num;
            stack.TryRemove(num);
        }
        numAdded = num;
        return stack;
    }

    // Removes number items from this stack and returns a new stack containing the removed items.
    public ItemStack TryRemove(int number)
    {
        Debug.Assert(number >= 0, "Cannot remove a negative amount");

        if (number == 0 || Count == 0)
        {
            return new ItemStack();
        }

        var item = Item;
        var numberToRemove = Mathf.Min(number, Count);

        Count -= numberToRemove;
        if (Count == 0)
        {
            Item = null;
            Capacity = DefaultStackCapacity;
        }

        return new ItemStack(item, numberToRemove);  
    }

    public ItemStack RemoveAll()
    {
        return TryRemove(Count);
    }
}
