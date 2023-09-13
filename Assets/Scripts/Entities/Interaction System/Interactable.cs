using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Receives interactions and sends them to all attached InteractionComponents.
/// </summary>
[RequireComponent(typeof(Hitbox))]
public class Interactable : MonoBehaviour, IPointerClickHandler
{
    public Transform Transform { get; private set; }
    public Vector2 Size { get; private set; }
    public Bounds Bounds { get => new Bounds(Transform.position, Size); }

    private List<IInteractionComponent> _interactionComponents { get; set; }

    [field: SerializeField] private Interactor _playerInteractor { get; set; }

    protected virtual void Awake()
    {
        Transform = transform;
        _interactionComponents = new List<IInteractionComponent>();
    }

    protected virtual void Start()
    {
        Size = GetComponent<Hitbox>().Size;
        IInteractionComponent[] interactionComponents = GetComponents<IInteractionComponent>();
        _interactionComponents.AddRange(interactionComponents);
    }

    public void ReceiveInteraction(Interaction interaction)  // todo: make this private, and handle all intercommunication code in the immediate interaction function call?
    {
        foreach (IInteractionComponent component in _interactionComponents)
        {
            component.ReceiveInteraction(interaction);
        }
    }

    // player function
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)  // TODO: change to work with input actions
            _playerInteractor.TryInteractWith(this);
        
    }
}
