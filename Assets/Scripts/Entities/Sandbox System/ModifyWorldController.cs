using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifyWorldController : MonoBehaviour, IUseSelectedItem
{
    [field: SerializeField] public int InventoryIdx { get; private set; }
    public Func<(Item, int)> GetItem { get; set; }  // using a Func here means that this class doesn't have access to all of Inventory's methods.
    //public event Action<int> OnConsume;

    private ModifyWorld ModifyWorld { get; set; }
    private ModifyOverlay Overlay { get; set; }

    public CarverHead BuildTool { get; set; }
    public CarverHead DestructTool { get; set; }
    public Tiles TileToBuildWith { get; set; }
    public float BuildSpeed { get; protected set; }
    public float DestructSpeed { get; protected set; }
    [field: SerializeField] protected CarverHead DefaultBuildTool { get; set; }
    [field: SerializeField] protected CarverHead DefaultDestructTool { get; set; }
    [field: SerializeField] public float DefaultBuildSpeed { get; protected set; }
    [field: SerializeField] public float DefaultDestructSpeed { get; protected set; }

    public bool OnBuildCooldown { get; protected set; }
    public bool OnDestructCooldown { get; protected set; }

    protected virtual void Awake()
    {
        //_inventories = GetComponent<Inventories>();
        ModifyWorld = GetComponent<ModifyWorld>();
        Overlay = GetComponentInChildren<ModifyOverlay>();
        BuildTool = DefaultBuildTool;
        DestructTool = DefaultBuildTool;

        TileToBuildWith = Tiles.Grass;
    }

    //private void Start()
    //{
    //    //Inventory = _inventories.inventories[_inventoryIdx];
    //}

    //private void OnEnable()
    //{
    //    if (HasInventory)
    //    {
    //        _inventory.ChangedSelectedItem += UpdateTools;
    //    }
    //}

    protected IEnumerator DoBuildCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnBuildCooldown = false;
    }

    protected IEnumerator DoDestructCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnDestructCooldown = false;
    }

    protected void TryBuild(Vector2 position, GeneratorManager.TileLayerIndices layerIndex)
    {
        Item item = null;
        int count = 0;
        if (GetItem != null)
        {
            (item, count) = GetItem.Invoke();
        }
        // TODO remove later
        if (item == null)
        {
            count = int.MaxValue;
        }
        
        UpdateTools(item);

        //Debug.Log($"{item}, {BuildTool}, {BuildSpeed}");

        if (BuildTool == null || BuildSpeed < 0)
        {
            return;
        }

        if (!OnBuildCooldown)
        {
            var nTilesChanged = ModifyWorld.Build(position, layerIndex, BuildTool, TileToBuildWith, count);
            //Debug.Log($"count: {count}, nTilesChanged: {nTilesChanged}");
            OnBuildCooldown = true;
            StartCoroutine(DoBuildCooldown(1 / (float)BuildSpeed));
            if (item != null)
            {
                //Debug.Log(item.GetInstanceID());
                item.OnBuildWith(nTilesChanged);
            }
        }
    }

    protected void TryDestruct(Vector2 position, GeneratorManager.TileLayerIndices layerIndex)
    {
        UpdateTools(GetItem?.Invoke().Item1);

        if (DestructTool == null || DestructSpeed < 0)
        {
            return;
        }

        if (!OnDestructCooldown)
        {
            ModifyWorld.Destruct(position, layerIndex, DestructTool);
            OnDestructCooldown = true;
            StartCoroutine(DoDestructCooldown(1 / (float)DestructSpeed));
        }
    }

    protected void UpdateTools(Item newHeldItem)
    {
        if (newHeldItem == null)
        {
            // set to defaults
            BuildTool = DefaultBuildTool;
            BuildSpeed = DefaultBuildSpeed;
            DestructTool = DefaultDestructTool;
            DestructSpeed = DefaultDestructSpeed;
            return;
        }
        if (newHeldItem.BuildComponent)
        {
            BuildTool = newHeldItem.BuildComponent.Tool;
            BuildSpeed = newHeldItem.BuildComponent.Speed;
            TileToBuildWith = newHeldItem.TileToBuildWith;
        }
        else
        {
            BuildTool = DefaultBuildTool;
            BuildSpeed = DefaultBuildSpeed;
            TileToBuildWith = Tiles.Grass;
        }

        if (newHeldItem.DestructComponent)
        {
            DestructTool = newHeldItem.DestructComponent.Tool;
            DestructSpeed = newHeldItem.DestructComponent.Speed;
        }
        else
        {
            DestructTool = DefaultDestructTool;
            DestructSpeed = DefaultDestructSpeed;
        }   
    }

    protected virtual void Update()
    {
        
        // update sizing for overlay object
        Overlay.UpdateSize(new Vector2Int(BuildTool.PrimarySize, BuildTool.SecondarySize));
    }



    //private void OnDisable()
    //{
    //    if (HasInventory)
    //    {
    //        _inventory.ChangedSelectedItem -= UpdateTools;
    //    }
    //}
}
