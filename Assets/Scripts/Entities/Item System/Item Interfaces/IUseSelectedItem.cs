using System;

public interface IUseSelectedItem : IUseInventory
{
    public Func<(Item, int)> GetItem { set; }
}
