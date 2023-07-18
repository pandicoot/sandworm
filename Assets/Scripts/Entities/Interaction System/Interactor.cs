using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform Transform { get; private set; }
    //[SerializeField] private InteractionManager _interactionManager;
    public Interactable Interactable { get; private set; }

    [field: SerializeField] private QuadtreeController _quadtreeController { get; set; }
    public List<Interactable> InteractablesInRange { get; private set; }

    [field: SerializeField] public float RangeToInteractWith { get; private set; }
    public Bounds InteractorBounds { get => new Bounds(Transform.position, new Vector2(RangeToInteractWith * 2, RangeToInteractWith * 2)); }

    private void Awake()
    {
        Transform = transform;
        InteractablesInRange = new List<Interactable>();
    }

    private void Start()
    {
        Interactable = GetComponent<Interactable>();
    }

    //public void TryInteractWith(Vector2 point)
    //{
    //    _interactionManager.SendInteraction(new Interaction(gameObject, this), point);
    //}

    private void Update()
    {
        InteractablesInRange.Clear();
        _quadtreeController.GetComponentInBounds<Interactable>(InteractorBounds, InteractablesInRange);
        InteractablesInRange.RemoveAll(x => Vector2.SqrMagnitude(x.Transform.position - Transform.position) > RangeToInteractWith * RangeToInteractWith);
    }

    public void TryInteractWith(Interactable interactable)
    {
        Debug.Assert(interactable != null, "interactable is null!");

        if (!InteractablesInRange.Contains(interactable))
        {
            return;
        }
        InteractWith(interactable);
    }

    private void InteractWith(Interactable interactable)
    {
        Debug.Assert(interactable != null, "trying to interact with null!");
        Debug.Assert(InteractablesInRange.Contains(interactable), "Trying to interact with an interactable not present in interactablesInRange list!");

        interactable.ReceiveInteraction(new Interaction(gameObject, this, interactable));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, RangeToInteractWith);
    }
}
