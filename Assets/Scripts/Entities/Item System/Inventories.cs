using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
    [field: SerializeField] public ItemManager ItemManager { get; private set; }

    public Inventory[] inventories { get; private set; }
    public int[] InventorySizes;
    public int NInventories { get => InventorySizes.Length; }

    [field: SerializeField] private InventoryItemDisplayFollow _follow { get; set; }
    private static readonly Vector2 _followSlotSize = new Vector2(8, 8);

    private bool _hasClicked { get; set; }

    private HashSet<IUseInventory>[] _inventoryUsers;

    private void Awake()
    {
        inventories = new Inventory[NInventories];
        for (int i = 0; i < NInventories; i++)
        {
            inventories[i] = new Inventory(InventorySizes[i]);
        }
        _inventoryUsers = new HashSet<IUseInventory>[NInventories];
        for (int i = 0; i < NInventories; i++)
        {
            _inventoryUsers[i] = new HashSet<IUseInventory>();
        }

        // injections (push up to EntityBase?)
        // temporary.
        // ideally only called once for each component, but injection methods should be idempotent
        var invSelectedItemUsers = GetComponents<IUseSelectedItem>();
        Array.ForEach(invSelectedItemUsers, c => InjectInventory(c));
        //var invConsumers = GetComponents<IConsumeSelectedItem>();
        //Array.ForEach(invConsumers, c => InjectInventory(c));
        //var invAttackers = GetComponents<IAttackWithSelectedItem>();
        //Array.ForEach(invAttackers, a => InjectInventory(a));
    }

    private void Start()
    {
        _follow.gameObject.SetActive(false);
        var followRT = _follow.transform as RectTransform;
        followRT.sizeDelta = _followSlotSize;


        inventories[0].AddItem(new ItemStack(ItemManager.GetNewItem("bronze_pickaxe"), 1));
        inventories[0].AddItem(new ItemStack(ItemManager.GetNewItem("bronze_shortsword"), 1));
        inventories[0].AddItem(new ItemStack(ItemManager.GetNewItem("bow"), 1));
    }

    public void InjectInventory(IUseInventory invUser)
    {
        var idx = invUser.InventoryIdx;
        if (!_inventoryUsers[idx].Contains(invUser))
        {
            _inventoryUsers[idx].Add(invUser);
        }
    }
    public void InjectInventory(IUseSelectedItem invUser)  
    {
        InjectInventory((IUseInventory)invUser);
        invUser.GetItem = inventories[invUser.InventoryIdx].GetSelected;
    }
    //public void InjectInventory(IConsumeSelectedItem invUser)
    //{
    //    InjectInventory((IUseSelectedItem)invUser);
    //    invUser.OnConsume -= inventories[invUser.InventoryIdx].OnConsumeSelected;  
    //    invUser.OnConsume += inventories[invUser.InventoryIdx].OnConsumeSelected;  
    //}
    //public void InjectInventory(IAttackWithSelectedItem invUser)
    //{
    //    InjectInventory((IUseSelectedItem)invUser);
    //    invUser.OnTryAttackWith -= inventories[invUser.InventoryIdx].OnAttackWith;
    //    invUser.OnTryAttackWith += inventories[invUser.InventoryIdx].OnAttackWith;
    //}


    public void OnClickedItemDisplay(int inventoryIdx, int slotIdx)
    {
        var inv = inventories[inventoryIdx];
        //Debug.Log("click");
        Debug.Assert(inv.Slots[slotIdx] != null, "An ItemStack in inventory is null");

        if (!_hasClicked && inv.Slots[slotIdx].Item == null)
        {
            return;
        }

        if (!_hasClicked)
        {
            ItemStack stack = inv.RemoveStack(slotIdx);
            if (stack.Count == 0)
            {
                return;
            }

            _hasClicked = true;

            // item sprite follow mouse
            _follow.ItemStack = stack;
            _follow.SetToMousePosition();
            _follow.gameObject.SetActive(true);
        }
        else
        { 
            var temp = inv.Slots[slotIdx];
            inv.Slots[slotIdx] = _follow.ItemStack;
            _follow.ItemStack = temp;
            if (_follow.ItemStack.Count == 0)
            {
                _hasClicked = false;
                _follow.gameObject.SetActive(false);
            }
        }
        
    }

    public bool TryPickup(ItemEntity itemEntity)
    {
        var fullyPickedUp = false;
        ItemStack stackAfterAdd = new ItemStack();
        for (int i = NInventories - 1; i >= 0; i--)  // pickup inventory priority order is opposite inventory order
        {
            stackAfterAdd = inventories[i].AddItem(itemEntity.ItemStack);
            Debug.Assert(stackAfterAdd != null, "stack after add is null!");
            if (stackAfterAdd.Count == 0)
            {
                fullyPickedUp = true;
                break;
            }
        }
        Debug.Log(stackAfterAdd);
        itemEntity.ItemStack = stackAfterAdd;

        if (fullyPickedUp)
        {
            Destroy(itemEntity.gameObject);  // TODO: can try to move destruction code to the ItemStack setter when count == 0
        }
        return fullyPickedUp;
    }

    //private void Update()
    //{
    //    Array.ForEach<Inventory>(inventories, inv => inv.UpdateInventory());

    //    //Debug.Log(_follow.ItemStack);
    //    //for (int i = 0; i < NInventories; i++)
    //    //{
    //    //    string s = $"Inventory {i}: \n";
    //    //    for (int j = 0; j < inventories[i].InventorySize; j++)
    //    //    {
    //    //        s += $"{inventories[i].Slots[j]}, ";
    //    //    }
    //    //    Debug.Log(s);
    //    //}
    //}
}
