using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractorController : MonoBehaviour
{
    public Interactor Interactor { get; protected set; }

    protected virtual void Start()
    {
        Interactor = GetComponent<Interactor>();
    }

    
}
