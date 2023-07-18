using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActiveAttacker))]
public abstract class AttackerController : MonoBehaviour
{
    protected ActiveAttacker _attacker;

    public bool AttackInput { get; protected set; } = false;

    private void Start()
    {
        _attacker = GetComponent<ActiveAttacker>();
    }

    protected virtual void TryAttack()
    {
        _attacker.TryMakeAttack();
    }

    protected abstract void GetAttackInput();

    private void Update()
    {
        GetAttackInput();

        if (AttackInput)
        {
            TryAttack();
        }
    }
}
