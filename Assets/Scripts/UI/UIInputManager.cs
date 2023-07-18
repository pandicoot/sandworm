using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    [field: SerializeField] private Canvas _canvas { get; set; }
    private PanelShowHide[] _panelOptions { get; set; }

    [field: SerializeField] private Canvas _entityCanvas { get; set; }
    private List<UIInteractable> _UIInteractables = new List<UIInteractable>();

    private bool _isUIOpen { get; set; }
    public bool IsUIActive { get; private set; }

    public event Action OpenedUI;
    public event Action ClosedUI;

    private void Awake()
    {
        _panelOptions = GetComponentsInChildren<PanelShowHide>();
    }

    private void Update()  // quick and dirty
    {
        _isUIOpen = false;
        foreach (PanelShowHide panel in _panelOptions)
        {
            if (panel.IsOpen)
            {
                _isUIOpen = true;
                break;
            }
        }
        
            
        if (_isUIOpen && !IsUIActive)
        {
            EnterUI();
        }
        else if (!_isUIOpen && IsUIActive)
        {
            ExitUI();
        }
    }

    public void OnPointerClickedInteractable(UIInteractable interactableClickedOn, PointerEventData eventData)
    {
        //IsUIActive = true;
        Debug.Log("interactable clicked");
    }

    public void EnterUI()
    {
        InputManager.SetMode(InputMode.UI);
        IsUIActive = true;
        OpenedUI?.Invoke();
    }

    public void CloseAndExitUI()
    {
        Debug.Log($"exit panel triggered. Current state of UI: {IsUIActive}");
        if (IsUIActive)
        {
            foreach (PanelShowHide panel in _panelOptions)
            {
                panel.Hide();
            }
            ExitUI();
        }
    }

    public void ExitUI()
    {
        InputManager.SetMode(InputMode.Gameplay);
        IsUIActive = false;
        ClosedUI?.Invoke();
    }

    public UIInteractable InstantiateInteractable(UIInteractable element, UIEntity backingEntity, Interactor interactor, Vector2 size, Vector2 position, Quaternion rotation)
    {
        Debug.Assert(!_UIInteractables.Contains(element), "List already contains this UIInteractable!");

        var uiInteractable = Instantiate<UIInteractable>(element, position, rotation, _entityCanvas.transform);
        uiInteractable.Entity = backingEntity;
        uiInteractable.UIInteractor = interactor;
        RectTransform rt = uiInteractable.transform as RectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

        _UIInteractables.Add(element);

        return uiInteractable;
    }

    public void RemoveInteractable(UIInteractable element)
    {
        if (!_UIInteractables.Contains(element))
        {
            return;
        }

        _UIInteractables.Remove(element);
    }
}
