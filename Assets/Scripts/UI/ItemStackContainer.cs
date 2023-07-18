using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class ItemStackContainer : MonoBehaviour
{
    private ItemStack _itemStack = new ItemStack();
    public ItemStack ItemStack
    {
        get => _itemStack;
        set
        {
            Debug.Assert(value != null, $"Trying to set ItemStack in {this} to be null.");

            ItemTexture = NoItemTexture;

            if (value.Item)
            {
                ItemTexture = value.Item.Sprite;
            }
            
            _itemStack = value;
            UpdateTexture(ItemTexture);
        }
    }

    protected Sprite ItemTexture { get; set; }
    [field: SerializeField] protected Sprite NoItemTexture { get; set; }

    protected abstract void UpdateTexture(Sprite texture);
}
