using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDisplayGrid : MonoBehaviour
{
    private GridLayoutGroup _gridLayoutGroup;

    [field: SerializeField] private Inventories _inventories { get; set; }
    [field: SerializeField] private int _inventoryIndex { get; set; }
    public Inventory Inventory { get; private set; }

    [field: SerializeField] private InventoryItemDisplaySlot _inventoryItemDisplaySlot { get; set; }
    [field: SerializeField] private InventoryItemDisplay _inventoryItemDisplay { get; set; }
    [field: SerializeField] private ItemStackCounterDisplay _itemStackCounterDisplay { get; set; }
    //[field: SerializeField] private InventoryItemDisplayFollow _follow { get; set; }

    public InventoryItemDisplaySlot[] ItemSlots { get; private set; }
    public InventoryItemDisplay[] ItemDisplays { get; private set; }

    private int _clickedDispIdx { get; set; } = -1;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        Debug.Assert(_inventoryIndex >= 0 && _inventoryIndex < _inventories.NInventories, "Inventory idx out of bounds");

        Inventory = _inventories.inventories[_inventoryIndex];

        // create slots and displays
        ItemSlots = new InventoryItemDisplaySlot[Inventory.InventorySize];
        ItemDisplays = new InventoryItemDisplay[Inventory.InventorySize];
        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            ItemSlots[i] = Instantiate(_inventoryItemDisplaySlot, transform);
            ItemDisplays[i] = Instantiate(_inventoryItemDisplay, ItemSlots[i].transform);
            var rt = ItemDisplays[i].transform as RectTransform;
            rt.sizeDelta = Vector2.zero;

            ItemDisplays[i].Grid = this;

            Instantiate(_itemStackCounterDisplay, ItemDisplays[i].transform);
        }

        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            ItemDisplays[i].ItemStack = Inventory.Slots[i];
        }
        //Inventory.MovedItems += UpdateDisplay;

        
    }

    //private void UpdateDisplay(int from, int to)
    //{
    //    var temp = ItemDisplays[to].ItemStack;
    //    ItemDisplays[to].ItemStack = ItemDisplays[from].ItemStack;
    //    ItemDisplays[from].ItemStack = temp;
    //}

    //private void OnEnable()
    //{
        
    //}

    //private void OnDisable()
    //{
    //    Inventory.MovedItems -= UpdateDisplay;
    //}

    public void OnClickedItemDisplay(InventoryItemDisplay disp)
    {
        var slotIdx = Array.IndexOf(ItemDisplays, disp);
        _inventories.OnClickedItemDisplay(_inventoryIndex, slotIdx);

        //Debug.Log("click");
        /*Debug.Assert(disp.ItemStack != null, "An ItemStack in inventory is null");

        if (_clickedDispIdx == -1 && disp.ItemStack.Item == null)
        {
            return;
        }

        var dispIdx = Array.IndexOf(ItemDisplays, disp);
        if (_clickedDispIdx == -1)
        {
            ItemStack stack = Inventory.RemoveStack(dispIdx);
            if (stack == null)
            {
                return;
            }
            _clickedDispIdx = dispIdx;

            // item sprite follow mouse
            _follow.ItemStack = stack;
            _follow.SetToMousePosition();
            _follow.gameObject.SetActive(true);

            return;
        }

        var heldStack = _follow.ItemStack;
        Inventory.SwapStacks(dispIdx, ref heldStack);
        _clickedDispIdx = -1;
        _follow.gameObject.SetActive(false);*/
    }

    private void Update()  
    {
        //// track item entities in slots
        for (int i = 0; i < Inventory.InventorySize; i++)
        {
            ItemDisplays[i].ItemStack = Inventory.Slots[i];
        }
    }
}
