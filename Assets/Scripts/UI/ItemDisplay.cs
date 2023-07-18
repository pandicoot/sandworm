using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    private TextMeshProUGUI _display;
    [field: SerializeField] private Inventories _playerInventories { get; set; }
    [field: SerializeField] private int _inventoryIdx { get; set; }

    private void Start()
    {
        _display = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateText(Item newHeldItem)
    {
        if (newHeldItem == null)
        {
            _display.text = "none";
            return;
        }
        _display.text = newHeldItem.Name;
    }

    private void Update()
    {
        UpdateText(_playerInventories.inventories[_inventoryIdx].Selected);
    }

    //private void OnEnable()
    //{
    //    _playerInventories.inventories[_inventoryIdx].ChangedSelectedItem += UpdateText;
    //}

    //private void OnDisable()
    //{
    //    _playerInventories.inventories[_inventoryIdx].ChangedSelectedItem -= UpdateText;
    //}
}
