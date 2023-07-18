using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEntity : ItemStackContainer
{
    public ItemManager ItemManager { get; set; }
    private SpriteRenderer _sr { get; set; }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateTexture(Sprite texture)
    {
        _sr.sprite = texture;
    }

    

}
