using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [field: SerializeField] private EntityManager _entityManager;

    [field: SerializeField] private List<Item> Items { get; set; }

    [field: SerializeField] private ItemEntity _itemEntity { get; set; }

    private Dictionary<Tiles, int> TileToItemIdx = new Dictionary<Tiles, int>()
    {
        {Tiles.Grass, 4 },
        {Tiles.Dirt, 5 },
        {Tiles.Stone, 6 },
        {Tiles.Sand, 7 },
        {Tiles.Darkstone, 8 },
        {Tiles.WoodenPlanks, 9 },
        {Tiles.StoneBricks, 10 },
        {Tiles.DirtWall, 11 },
        {Tiles.StoneWall, 12 },
        {Tiles.Granite, 13 },
        {Tiles.Marble, 14 },
        {Tiles.Mud, 15 },
        {Tiles.Mudstone, 16 },
    };

    public event Action<Item> CreatedNewItem;
    public event Action<IInstantiateEntity> CreatedNewInstantiator;

    private List<ModifyWorld> _sandboxAgents = new List<ModifyWorld>();
    public List<ItemEntity> SpawnedItemEntities { get; private set; } = new List<ItemEntity>();

    private Action<GeneratorManager.TileLayerIndices, Vector2, HashSet<(Vector2Int, Tiles)>> _instantiateItemEntityUponTileDestroy;

    private void Awake()
    {
        _instantiateItemEntityUponTileDestroy = (idx, originPos, destroyedTiles) =>
        {
            foreach ((Vector2Int pos, Tiles tile) in destroyedTiles)
            {
                CreateItemEntity(new ItemStack(GetTileItem(tile), 1), ChunkManager.ToWorldPosition(pos));
            }
        };
    }

    private void OnEnable()
    {
        _entityManager.AddedNewEntity += _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity += _entityManager_RemovedEntity;
    }

    private void _entityManager_AddedNewEntity(GameObject obj)
    {
        if (obj.TryGetComponent<ModifyWorld>(out ModifyWorld component))
        {
            _sandboxAgents.Add(component);
            component.DestroyedAt += _instantiateItemEntityUponTileDestroy;
        }
    }

    private void _entityManager_RemovedEntity(GameObject obj)
    {
        if (obj.TryGetComponent<ModifyWorld>(out ModifyWorld component))
        {
            _sandboxAgents.Remove(component);
            component.DestroyedAt -= _instantiateItemEntityUponTileDestroy;
        }

        if (obj.TryGetComponent<ItemEntity>(out ItemEntity itemEntity))
        {
            SpawnedItemEntities.Remove(itemEntity);
        }
    }

    private void OnDisable()
    {
        _entityManager.AddedNewEntity -= _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity -= _entityManager_RemovedEntity;
    }

    private void Start()
    {
        foreach (GameObject entity in _entityManager.Entities)
        {
            if (entity.TryGetComponent<ModifyWorld>(out ModifyWorld component))
            {
                _sandboxAgents.Add(component);
                component.DestroyedAt += _instantiateItemEntityUponTileDestroy;
            }
        }
    }

    public Item GetNewItem(string name)
    {
        var itemPrototype = (Item)Items.Find(x => x.Name == name);
        Debug.Assert(itemPrototype != null, $"Could not find item with the name {name}");
        var item = (Item)itemPrototype.Clone();

        CreatedNewItem?.Invoke(item);

        foreach (ItemComponent c in item.Components)
        {
            var instantiator = c as IInstantiateEntity;
            if (instantiator != null)
            {
                CreatedNewInstantiator?.Invoke(instantiator);
            }
        }

        return item;
    }

    public Item GetNewItem(int idx)
    {
        var item = (Item)Items[idx].Clone();

        CreatedNewItem?.Invoke(item);

        foreach (ItemComponent c in item.Components)
        {
            var instantiator = c as IInstantiateEntity;
            if (instantiator != null)
            {
                CreatedNewInstantiator?.Invoke(instantiator);
            }
        }

        return item;
    }

    public Item GetTileItem(Tiles tile)
    {
        Debug.Assert(TileToItemIdx.ContainsKey(tile), "This tile is not present in the tileToItemIdx dictionary");
        var item = (Item)Items[TileToItemIdx[tile]].Clone();
        CreatedNewItem?.Invoke(item);
        return item;
    }

    public void CreateItemEntity(ItemStack itemStack, Vector2 position)
    {
        var itemEntity = Instantiate<ItemEntity>(_itemEntity, position, Quaternion.identity);
        itemEntity.ItemStack = itemStack;
        _entityManager.AddNewEntity(itemEntity.gameObject);

        SpawnedItemEntities.Add(itemEntity);
        itemEntity.ItemManager = this;
    }
}
