using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives interactions and sends them to all attached InteractionComponents.
/// </summary>
[RequireComponent(typeof(Hitbox))]
public class Interactable : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Vector2 Size { get; private set; }
    public Bounds Bounds { get => new Bounds(Transform.position, Size); }

    private List<IInteractionComponent> _interactionComponents { get; set; }

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

    public void ReceiveInteraction(Interaction interaction)
    {
        foreach (IInteractionComponent component in _interactionComponents)
        {
            component.ReceiveInteraction(interaction);
        }
    }
}
