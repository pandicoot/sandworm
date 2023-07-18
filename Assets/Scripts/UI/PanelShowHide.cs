using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PanelShowHide : MonoBehaviour
{
    [field: SerializeField] private string ButtonName { get; set; }
    private InputAction _toggle { get; set; }
    private CanvasGroup _group { get; set; }

    public bool IsOpen
    {
        get
        {
            if (_group != null)
            {
                return _group.interactable;
            }
            else
            {
                return false;
            }
        }
    }

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _toggle = InputManager.Actions.FindAction(ButtonName);
        Debug.Assert(_toggle != null, "Incorrect button name or toggle does not exist.");
    }

    private void OnEnable()
    {
        _toggle.performed += _toggle_performed;
        
    }
    private void OnDisable()
    {
        _toggle.performed -= _toggle_performed;
    }

    private void _toggle_performed(InputAction.CallbackContext obj)
    {
        Toggle();
    }

    public void Show()
    {
        if (_group != null)
        {
            _group.interactable = true;
            _group.alpha = 1;
            _group.blocksRaycasts = true;
        }
    }

    public void Hide()
    {
        if (_group != null)
        {
            _group.interactable = false;
            _group.alpha = 0;
            _group.blocksRaycasts = false;
        }
    }
    public void Toggle()
    {
        if (!IsOpen)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
