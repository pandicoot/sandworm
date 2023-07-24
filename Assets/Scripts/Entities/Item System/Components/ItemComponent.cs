using System;

public abstract class ItemComponent : Prototype
{
    protected Item _item;
    public virtual Item Item { get => _item; set => _item = value; }

    //private IItemComponentData _originalData;
    //public abstract IItemComponentData OriginalData { get; set; }

    public ItemComponent() { }

    public event Action<int> RequestRemoveItem;
    protected void OnRequestRemoveItem(int n)
    {
        RequestRemoveItem?.Invoke(n);
    }

    public virtual void OnAttackWith(Attack atk) { }
    public virtual void OnBuildWith(int n) { }
    public virtual void OnDestructWith() { }
    public virtual void OnUseWith() { }
}
