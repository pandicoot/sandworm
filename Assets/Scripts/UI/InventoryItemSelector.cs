using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSelector : MonoBehaviour
{
    [field: SerializeField] public InventoryItemDisplayGrid Grid { get; private set; }

    public Inventory Inventory { get; private set; }
    private InventoryItemDisplay[] _displays { get; set; }
    private InventoryItemDisplay _currentlySelectedDisplay { get; set; }
    private Transform _transform { get; set; }


    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _transform.position = Grid.transform.position;
        //_displays = GetComponentsInChildren<InventoryItemDisplay>();
    }

    private void Update()
    {
        if (Grid.Inventory == null)
        {
            return;
        }
        if (Inventory == null)
        {
            Inventory = Grid.Inventory;
        }
        if (_displays == null)
        {
            _displays = Grid.GetComponentsInChildren<InventoryItemDisplay>();
        }

        if (_currentlySelectedDisplay != null)
        {
            _currentlySelectedDisplay.IsSelected = false;
        }
        _currentlySelectedDisplay = _displays[Inventory.IndexOfSelectedItem];
        _currentlySelectedDisplay.IsSelected = true;

        _transform.position = _currentlySelectedDisplay.transform.position;
    }
}
