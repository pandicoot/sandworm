using System;

public interface IConsumeSelectedItem : IUseSelectedItem  // TODO: replace with ISecondaryUseSelectedItem?
{
    public event Action<int> OnConsume;
}
