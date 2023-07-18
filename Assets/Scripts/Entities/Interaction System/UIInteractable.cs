using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Receives UI events and interprets them as Interact calls by the designated
/// UIInteractor to the Interactable coupled to this UI element.
/// </summary>
public class UIInteractable : MonoBehaviour, IPointerClickHandler
{
    public UIEntity Entity { get; set; }
    public Interactor UIInteractor { get; set; }
    private UIInputManager _UIInputManager;
    private CanvasScaler _canvasScaler { get; set; }
    
    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        _canvasScaler = GetComponentInParent<CanvasScaler>();
        _UIInputManager = GetComponentInParent<UIInputManager>();
    }

    private void Update()
    { 
        RectTransform.position = Entity.Transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _UIInputManager.OnPointerClickedInteractable(this, eventData);
        Debug.Log("OnPointerClick");
        UIInteractor.TryInteractWith(Entity.Interactable);

    }

}
