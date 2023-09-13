using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UIInputManager _uiManager;
    private Image _image;

    public event Action ClickedBackground;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedBackground?.Invoke();
        _uiManager.CloseAndExitUI();
        
    }

    private void Update()
    {
        _image.raycastTarget = _uiManager.IsUIActive;
    }
}
