using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public GameObject InteractorGameObject { get; }
    public Interactor Interactor { get; }
    public Interactable Interactable { get; }

    public Interaction(GameObject interactorGameObject, Interactor interactor, Interactable interactable)
    {
        InteractorGameObject = interactorGameObject;
        Interactor = interactor;
        Interactable = interactable;
    }
}