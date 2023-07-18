using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeElement : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public Attackable Attackable { get; private set; }
    public Interactable Interactable { get; private set; }
    public QuadtreeNode Node { get; set; }  // need better protection for this one

    private void Awake()
    {
        Transform = transform;
        Attackable = GetComponent<Attackable>();
        Interactable = GetComponent<Interactable>();
    }
}
