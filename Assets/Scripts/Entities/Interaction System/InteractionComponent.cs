using UnityEngine;

/// <summary>
/// An interface for receiving interactions from the Interactable object attached to this MonoBehaviour.
/// </summary>
public interface IInteractionComponent 
{
    public void ReceiveInteraction(Interaction interaction);
}
