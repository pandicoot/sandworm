using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    private Inventories _inventories { get; set; }
    private Inventory _inventory { get; set; }
    [field: SerializeField] private int _inventoryIdx { get; set; }

    private void Start()
    {
        _inventories = GetComponent<Inventories>();
        _inventory = _inventories.inventories[_inventoryIdx];
    }

    private void Update()
    {
        if (InputManager.Actions.ui.hold_next.WasPressedThisFrame())
        {
            _inventory.HoldNextItem();
        }
        else if (InputManager.Actions.ui.hold_prev.WasPressedThisFrame())
        {
            _inventory.HoldPrevItem();
        }
    }
}
