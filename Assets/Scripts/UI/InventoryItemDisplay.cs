using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// The UI display of an item in a slot in an inventory.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemDisplay : ItemStackContainer, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool _pointerOn { get; set; }

    public InventoryItemDisplayGrid Grid { get; set; }

    //private ItemStack _itemStack;
    //public ItemStack ItemStack
    //{
    //    get => _itemStack;
    //    set
    //    {
    //        _itemStack = value;
    //        if (_itemStack.Item != null)
    //        {
    //            _image.sprite = _itemStack.Item.Sprite;
    //        }
    //        else
    //        {
    //            _image.sprite = NoItemTexture;
    //        }
    //    }
    //}

    //[field: SerializeField] private Sprite NoItemTexture { get; set; }

    private Image _image { get; set; }

    public bool IsSelected { get; set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        //if (Item != null) Debug.Log(Item);
        if (IsSelected)
        {
            
        }

        if (_pointerOn)
        {
            // Listen for key events

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("opc");
        Grid.OnClickedItemDisplay(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Assert(_pointerOn == true, "pointer bool is wrong");
        _pointerOn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Assert(_pointerOn == false, "pointer bool is wrong");
        _pointerOn = true;
    }

    protected override void UpdateTexture(Sprite texture)
    {
        _image.sprite = texture;
    }
}
