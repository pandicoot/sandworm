using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogPanel : MonoBehaviour
{
    [field: SerializeField] private TextMeshProUGUI _dialogText { get; set; }
    [field: SerializeField] private TextMeshProUGUI _dialogHeader { get; set; }
    private PanelShowHide _dialogPanelShowHide { get; set; }
    private DialogueInteractionComponent _controller { get; set; }
    public bool Registered { get => _controller != null; }

    private void Start()
    {
        _dialogPanelShowHide = GetComponent<PanelShowHide>();
        _dialogHeader.text = string.Empty;
        _dialogText.text = string.Empty;
    }

    private void UpdateText(string header, string text)
    {
        _dialogText.text = text;
        _dialogHeader.text = header;
        _dialogPanelShowHide.Show();
    }

    public void RegisterController(DialogueInteractionComponent controller)
    {
        if (Registered)
        {
            Deregister();
        }

        _controller = controller;
        _controller.UpdatedText += UpdateText;
        
        //return true;
    }

    public void Deregister()
    {
        if (!Registered)
        {
            return;
        }

        _dialogHeader.text = string.Empty;
        _dialogText.text = string.Empty;
        _controller.UpdatedText -= UpdateText;
        _controller = null;
        _dialogPanelShowHide.Hide();
    }

    public void Next()
    {
        if (!Registered)
        {
            return;
        }
        _controller.Next();
    }
}
