using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public sealed class Item : Prototype, IEquatable<Item>
{
    //public ItemData Data { get; }
    //public ItemComponent[] ItemComponents { get; private set; }

    private Item _originalItem { get; set; }

    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string[] Tags { get; private set; }

    [field: SerializeField] public int StackLimit { get; private set; }

    private List<ItemComponent> _components = new List<ItemComponent>();
    public List<ItemComponent> Components { get => _components; }  // should store a list of each component instance on this item.
    // won't work exactly for prototypical items, as these have multiple components mapped to the same asset instance

    [SerializeField] private WeaponComponent _weaponComponent;
    public WeaponComponent WeaponComponent { get => _weaponComponent;
        set
        {
            if (_weaponComponent)
            {
                _components.Remove(_weaponComponent);
                value.Item = null;
            }

            if (value)
            {
                _components.Add(value);
                value.Item = this;
            }
            _weaponComponent = value;
        }
    }

    [SerializeField] private ProjectileComponent _projectileComponent;
    public ProjectileComponent ProjectileComponent
    {
        get => _projectileComponent;
        set
        {
            if (_projectileComponent)
            {
                _components.Remove(_projectileComponent);
                value.Item = null;
            }

            if (value)
            {
                _components.Add(value);
                value.Item = this;
            }
            _projectileComponent = value;
        }
    }

    [SerializeField] private ToolComponent _buildComponent;
    public ToolComponent BuildComponent
    {
        get => _buildComponent;
        set
        {
            if (_buildComponent)
            {
                _components.Remove(_buildComponent);
                value.Item = null;
            }

            if (value)
            {
                _components.Add(value);
                value.Item = this;
            }
            _buildComponent = value;
        }
    }

    [SerializeField] private ToolComponent _destructComponent;
    public ToolComponent DestructComponent
    {
        get => _destructComponent;
        set
        {
            if (_destructComponent)
            {
                _components.Remove(_destructComponent);
                value.Item = null;
            }

            if (value)
            {
                _components.Add(value);
                value.Item = this;
            }
            _destructComponent = value;
        }
    }

    private void OnValidate()
    {
        WeaponComponent = _weaponComponent;
        ProjectileComponent = _projectileComponent;
        BuildComponent = _buildComponent;
        DestructComponent = _destructComponent;
        Debug.Log(Components.Count);
        Debug.Log(Components);
    }

    public Action<Attack> OnAttackWith;

    //public void AddComponent(ItemComponent newComponent) 
    //{
    //    if (!_components.Contains(newComponent))
    //    {
    //        _components.Add(newComponent);
    //        newComponent.Item = this;
    //    }
    //}
    //public void RemoveComponent(ItemComponent component)
    //{
    //    if (!_components.Contains(component))
    //    {
    //        return;
    //    }

    //    _components.Remove(component);
    //    component.Item = null;

    //    if (WeaponComponent == component)
    //    {
    //        WeaponComponent = null;
    //    }
    //    if (BuildComponent == component)
    //    {
    //        BuildComponent = null;
    //    }
    //    if (DestructComponent == component)
    //    {
    //        DestructComponent = null;
    //    }
    //}

    public override object Clone()
    {
        var newItem = ScriptableObject.CreateInstance<Item>();
        newItem.Name = Name;
        newItem.Sprite = Sprite;
        newItem.Description = Description;
        newItem.Tags = (string[])Tags.Clone();
        newItem._components = new List<ItemComponent>();
        if (WeaponComponent != null)
        {
            var newWeaponComponent = (WeaponComponent)WeaponComponent.Clone();
            newItem.WeaponComponent = newWeaponComponent;
        }
        if (ProjectileComponent != null)
        {
            var newProjectileComponent = (ProjectileComponent)ProjectileComponent.Clone();
            newItem.ProjectileComponent = newProjectileComponent;
        }
        if (DestructComponent != null)
        {
            var newDestructComponent = (ToolComponent)DestructComponent.Clone();
            newItem.DestructComponent = newDestructComponent;
        }
        if (BuildComponent != null)
        {
            var newBuildComponent = (ToolComponent)BuildComponent.Clone();
            newItem.BuildComponent = newBuildComponent;
        }

        return newItem;
    }



    //public Item(ItemData data)
    //{
    //    Data = data;
    //    ItemComponents = new ItemComponent[Data.Components.Length];

    //    for (int i = 0; i < Data.Components.Length; i++)
    //    {
    //        ItemComponents[i] = Data.Components[i].InstantiateComponent();
    //    }

    //    WeaponComponent = (WeaponComponent)Data.WeaponComponent.InstantiateComponent();

    //    DestructComponent = (ToolComponent)Data.DestructComponent.InstantiateComponent();
    //    BuildComponent = (ToolComponent)Data.BuildComponent.InstantiateComponent();
    //}

    //public T GetItemComponent<T>()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        if (component is T t)
    //        {
    //            return t;
    //        }
    //    }
    //    return default(T);
    //}

    //public void OnAcquire()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        component.OnAcquire();
    //    }
    //}
    //public void OnDrop()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        component.OnDrop();
    //    }
    //}
    //public void WhileInInventory()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        component.WhileInInventory();
    //    }
    //}
    //public void WhileInHotbar()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        component.WhileInHotbar();
    //    }
    //}
    //public void WhileHeld()
    //{
    //    foreach (ItemComponent component in ItemComponents)
    //    {
    //        component.WhileHeld();
    //    }
    //}
    //public void OnPrimaryUseWith()
    //{

    //}
    //public void OnSecondaryUseWith()
    //{

    //}
    //public void WhilePrimaryUseWith()
    //{

    //}
    //public void WhileSecondaryUseWith()
    //{

    //}

    public override bool Equals(object other) => this.Equals(other as Item);

    public bool Equals(Item other)  // doesn't check for equality of _originalItem or OnAttackWith
    {
        if (other == null)
        {
            return false;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        if (Name != other.Name ||
            Sprite != other.Sprite ||
            Description != other.Description ||
            !Tags.SequenceEqual(other.Tags) ||
            StackLimit != other.StackLimit)
        {
            return false;
        }
        // components
        if (WeaponComponent != other.WeaponComponent ||
            ProjectileComponent != other.ProjectileComponent ||
            BuildComponent != other.BuildComponent ||
            DestructComponent != other.DestructComponent)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Name);
        hash.Add(Sprite);
        hash.Add(Description);
        hash.Add(Tags);
        hash.Add(StackLimit);
        hash.Add(WeaponComponent);
        hash.Add(ProjectileComponent);
        hash.Add(BuildComponent);
        hash.Add(DestructComponent);
        return hash.ToHashCode();
    }

    public static bool operator ==(Item lhs, Item rhs)
    {
        if (object.ReferenceEquals(lhs, rhs))
        {
            return true;
        }
        if (object.ReferenceEquals(lhs, null))
        {
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Item lhs, Item rhs) => !(lhs == rhs);



    //public static bool operator ==(Item a, Item b)
    //{
    //    
    //}

    //public static bool operator !=(Item a, Item b) => !(a == b);
}
