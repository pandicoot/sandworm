using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreeController : MonoBehaviour
{
    private EntityManager _entityManager { get; set; }

    public Quadtree Quadtree { get; private set; }
    //private List<Transform> _entityTransforms { get; set; }
    private List<QuadtreeElement> _quadtreeElements { get; set; } = new List<QuadtreeElement>();

    private List<QuadtreeNode> _containingNodes { get; set; }

    private void Awake()
    {
        _entityManager = GetComponent<EntityManager>();
    }

    private void Start()
    {
        _quadtreeElements.AddRange(GetComponentsInChildren<QuadtreeElement>());

        // global quadtree
        float blockLength = GeneratorManager.LengthInChunkUnits * ChunkManager.ChunkSize.x * ChunkManager.TileSize;
        float blockHeight = GeneratorManager.HeightInChunkUnits * ChunkManager.ChunkSize.y * ChunkManager.TileSize;

        Quadtree = new Quadtree(new Vector2(blockLength / 2f, blockHeight / 2f), new Vector2(blockLength / 2f, blockHeight / 2f));
        for (int i = 0; i < _quadtreeElements.Count; i++)
        {
            Quadtree.Add(_quadtreeElements[i]);
        }

        _containingNodes = new List<QuadtreeNode>();
    }

    private void OnEnable()
    {
        _entityManager.AddedNewEntity += _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity += _entityManager_RemovedEntity;
    }

    private void _entityManager_AddedNewEntity(GameObject obj)
    {
        if (obj.TryGetComponent<QuadtreeElement>(out QuadtreeElement qElem))
        {
            _quadtreeElements.Add(qElem);
            Quadtree.Add(qElem);
        }
    }
    private void _entityManager_RemovedEntity(GameObject obj)
    {
        // assumes correctness of list
        if (obj.TryGetComponent<QuadtreeElement>(out QuadtreeElement qElem))
        {
            _quadtreeElements.Remove(qElem);
            Quadtree.Remove(qElem);
        }
    }

    private void OnDisable()
    {
        _entityManager.AddedNewEntity -= _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity -= _entityManager_RemovedEntity;
    }

    private void Update()
    {
        // assume all elements may need to be updated (are moving)
        _quadtreeElements.ForEach(x => Quadtree.ValidatePartitionOf(x));
        Quadtree.DrawBoundsInScene();
        
    }

    public void GetComponentInBounds<T>(Bounds bounds, List<T> output) where T : Component
    {
        Debug.Assert(output != null, "output list is null!");

        var elements = new List<QuadtreeElement>();
        _containingNodes.Clear();
        Quadtree.GetElementsInBounds(bounds, elements, _containingNodes);

        foreach (var elem in elements)
        {
            if (elem.TryGetComponent<T>(out T component))
            {
                output.Add(component);
            }
        }
        DrawContainingNodes();
    }
    // we use QuadtreeElement to store the components that are queried by the quadtree to avoid GetComponent calls
    //public void GetAttackablesInBounds(Bounds bounds, List<Attackable> attackables)
    //{
    //    Debug.Assert(attackables != null, "attackables list is null!");

    //    var elements = new List<QuadtreeElement>();
    //    _containingNodes.Clear();
    //    Quadtree.GetElementsInBounds(bounds, elements, _containingNodes);

    //    foreach (var elem in elements)
    //    {
    //        if (elem.Attackable != null)
    //        {
    //            attackables.Add(elem.Attackable);
    //        }
    //    }
    //    DrawContainingNodes();
    //}

    //public void GetInteractablesAtPoint(Vector2 point, List<Interactable> interactables)
    //{
    //    Debug.Assert(interactables != null, "interactables list is null!");

    //    var elements = new List<QuadtreeElement>();
    //    Quadtree.GetElementsAtPoint(point, elements, out QuadtreeNode containingNode);

    //    foreach (QuadtreeElement elem in elements)
    //    {
    //        if (elem.Interactable != null)
    //        {
    //            interactables.Add(elem.Interactable);
    //        }
    //    }
    //    DrawNode(containingNode);
    //}

    //public void GetInteractablesInBounds(Bounds bounds, List<Interactable> interactables)
    //{
    //    Debug.Assert(interactables != null, "interactables list is null!");

    //    var elements = new List<QuadtreeElement>();
    //    _containingNodes.Clear();
    //    Quadtree.GetElementsInBounds(bounds, elements, _containingNodes);

    //    foreach (var elem in elements)
    //    {
    //        if (elem.Interactable != null)
    //        {
    //            interactables.Add(elem.Interactable);
    //        }
    //    }
    //    DrawContainingNodes();
    //}

    private void DrawNode(QuadtreeNode node)
    {
        Debug.DrawLine(node.Bounds.min, node.Bounds.max, Color.magenta, 0.2f);
        Debug.DrawLine(node.Bounds.min + Vector3.up * node.Bounds.size.y, node.Bounds.max - Vector3.up * node.Bounds.size.y, Color.magenta, 0.2f);
    }

    private void DrawContainingNodes()
    {
        foreach (QuadtreeNode node in _containingNodes)
        {
            DrawNode(node);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Debug.Log(_containingNodes);
    //    foreach (QuadtreeNode node in _containingNodes)
    //    {
    //        Gizmos.DrawCube(node.Bounds.center, node.Bounds.size);
    //    }
    //}


}
