using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates and couples to a UI element on the Canvas shadowing this object, which is
/// receptive to UI events.
/// </summary>
[RequireComponent(typeof(Hitbox))]
public class UIEntity : MonoBehaviour
{
    //[field: SerializeField] private Canvas _canvas { get; set; }
    [field: SerializeField] private UIInputManager _UIInputManager { get; set; }
    [field: SerializeField] private UIInteractable _UIElement { get; set; }
    public Interactor _UIInteractor { get; private set; }  // The Interactor object that this UIEntity will be receptive to.
    public Interactable Interactable { get; private set; }
    public Transform Transform { get; private set; }
    public Vector2 Size { get; private set; }

    private void Awake()
    {
        Transform = transform;
        Interactable = GetComponent<Interactable>();
    }

    private void Start()
    {
        _UIInteractor = GameObject.FindWithTag("Player").GetComponent<Interactor>();

        Size = GetComponent<Hitbox>().Size;

        //UIInteractable uiInteractable = Instantiate<UIInteractable>(_UIElement, transform.position, Quaternion.identity, _canvas.transform);
        //uiInteractable.Entity = this;
        //uiInteractable.UIInteractor = _UIInteractor;
        //RectTransform rt = uiInteractable.transform as RectTransform;
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Size.x);
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Size.y);

        _UIInputManager.InstantiateInteractable(_UIElement, this, _UIInteractor, Size, transform.position, Quaternion.identity);
    }
}
