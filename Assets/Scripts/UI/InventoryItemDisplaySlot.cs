using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// The UI display of a slot in an inventory.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemDisplaySlot : MonoBehaviour, IPointerClickHandler
{
    public bool IsSelected { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
