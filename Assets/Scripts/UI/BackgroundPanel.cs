using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UIInputManager _uiManager;

    public event Action ClickedBackground;

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickedBackground?.Invoke();
        _uiManager.CloseAndExitUI();
        
    }
}
