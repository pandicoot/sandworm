using System;

public interface IAttackWithSelectedItem : IUseSelectedItem
{
    public event Action<Attack> OnTryAttackWith;
}
