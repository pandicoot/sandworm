using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack_O 
{
    public const int DefaultStackCapacity = 81;
    public Stack<Item> items = new Stack<Item>();
    private int _capacity;
    public int Capacity
    {
        get => _capacity;
        set
        {
            Debug.Assert(value >= 0, "Capacity cannot be zero or negative");
            _capacity = value;
        }
    }
    public bool ShowStackIndicator { get; private set; }

    private void Acquire(Item item)
    {
        Debug.Assert(items.Count == 0);

        if (item.StackLimit > 0)
        {
            Capacity = item.StackLimit;
        }
        else
        {
            Capacity = DefaultStackCapacity;
        }

        ShowStackIndicator = item.StackLimit != 1;

        items.Push(item);
    }

    public bool Add(Item item)
    {
        var res = true;
        if (items.Count == 0)
        {
            Acquire(item);
        }
        else if (items.Count < Capacity && item == items.Peek())
        {
            items.Push(item);
        }
        else
        {
            res = false;
        }
        return res;
    }

    public void Remove(int number)
    {
        Debug.Assert(number >= 0, "Cannot remove a negative amount");

        if (number == 0 || items.Count == 0) return;

        var numberToRemove = Mathf.Min(number, items.Count);

        if (numberToRemove == items.Count)
        {
            items.Clear();
            return;
        }

        for (int i = 0; i < numberToRemove; i++)
        {
            items.Pop();
        }

    }
}
