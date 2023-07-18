using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItemDisplayFollow : ItemStackContainer
{
    //[field: SerializeField] private UIInputManager _uiManager { get; set; }
    [field: SerializeField] private BackgroundPanel _background { get; set; }
    [field: SerializeField] private ItemManager _itemManager { get; set; }
    private Camera _cam;

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
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        //_uiManager.ClosedUI += DropHeldItem;
        _background.ClickedBackground += DropHeldItem;
    }

    private void DropHeldItem()
    {
        Debug.Log(transform.position);
        if (ItemStack != null)
        {
            if (ItemStack.Item != null)
            {
                _itemManager.CreateItemEntity(ItemStack, _cam.ScreenToWorldPoint(InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>()));
                ItemStack = new ItemStack();
            }
        }
    }

    private void OnDisable()
    {
        //_uiManager.ClosedUI -= DropHeldItem;
        _background.ClickedBackground -= DropHeldItem;
    }

    public void SetToMousePosition()
    {
        transform.position = InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    { 
        SetToMousePosition();
    }

    protected override void UpdateTexture(Sprite texture)
    {
        _image.sprite = texture;
    }
}
