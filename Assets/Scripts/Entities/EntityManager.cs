using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [field: SerializeField] private ItemManager _itemManager { get; set; }

    public List<GameObject> Entities { get; private set; } = new List<GameObject>();

    private List<IInstantiateEntity> _instantiators { get; set; } = new List<IInstantiateEntity>();

    public event Action<GameObject> AddedNewEntity;
    public event Action<GameObject> RemovedEntity;

    public QuadtreeController QuadtreeController { get; private set; }

    private void Awake()
    {
        QuadtreeController = GetComponent<QuadtreeController>();
        foreach (Transform child in transform)
        {
            Entities.Add(child.gameObject);
            SubscribeToOnDestroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        _itemManager.CreatedNewInstantiator += AddNewInstantiator;
        _instantiators.ForEach(inst => inst.Instantiated += AddNewEntity);
    }

    private void OnDisable()
    {
        _itemManager.CreatedNewInstantiator -= AddNewInstantiator;
        _instantiators.ForEach(inst => inst.Instantiated -= AddNewEntity);
    }

    private void SubscribeToOnDestroy(GameObject entity)
    {
        var entityBase = entity.GetComponent<EntityBase>();
        Debug.Assert(entityBase != null, $"No EntityBase component for {entity}!");
        entityBase.WasDestroyed += RemoveEntity;
    }
    private void DesubscribeFromOnDestroy(GameObject entity)
    {
        var entityBase = entity.GetComponent<EntityBase>();
        Debug.Assert(entityBase != null, $"No EntityBase component for {entity}!");
        entityBase.WasDestroyed -= RemoveEntity;
    }

    private void AddNewInstantiator(IInstantiateEntity inst)
    {
        inst.Instantiated += AddNewEntity;
        _instantiators.Add(inst);
    }

    public void AddNewEntity(GameObject entity)
    {
        bool contains = Entities.Contains(entity);
        Debug.Assert(contains == entity.transform.IsChildOf(transform), "Entity list is not synchronised");
        Debug.Assert(!contains, $"{entity} already in list");

        entity.transform.parent = transform;
        Entities.Add(entity);

        SetEntityData(entity);

        SubscribeToOnDestroy(entity);

        AddedNewEntity?.Invoke(entity);
    }

    public void RemoveEntity(GameObject entity)
    {
        bool contains = Entities.Contains(entity);
        Debug.Assert(contains == entity.transform.IsChildOf(transform), "Entity list is not synchronised");

        if (contains)
        {
            Entities.Remove(entity);
            //entity.transform.parent = transform.parent;  // TODO return to pool
            RemovedEntity?.Invoke(entity);

            DesubscribeFromOnDestroy(entity);
        }
    }

    public void SetEntityData(GameObject entity)
    {
        if (entity.TryGetComponent<PassiveAttacker>(out PassiveAttacker passiveAttacker))
        {
            passiveAttacker.QuadtreeController = QuadtreeController;
        }
        if (entity.TryGetComponent<ActiveAttacker>(out ActiveAttacker activeAttacker))
        {
            activeAttacker.QuadtreeController = QuadtreeController;
        }
    }

    public List<T> GetComponentsInEntities<T>() where T : UnityEngine.Component
    {
        var components = new List<T>();
        foreach (GameObject e in Entities)
        {
            if (e.TryGetComponent<T>(out T component))
            {
                components.Add(component);
            }
        }
        return components;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Entities.Count);
    }
}
