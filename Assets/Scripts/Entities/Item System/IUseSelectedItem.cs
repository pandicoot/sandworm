using System;

public interface IUseSelectedItem : IUseInventory
{
    public Func<Item> GetItem { set; }
}
