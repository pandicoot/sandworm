using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Facing))]
public class ActiveAttacker : MonoBehaviour, IAttacker
{
    public event Action<Attack> OnAttack;

    public Transform Transform { get; private set; }
    public QuadtreeController QuadtreeController { get; set; }
    public EntityAttackStats AttackStats { get; private set; }
    [field: SerializeField] public EntityAttackStats BaseAttackStats { get; private set; }
    public EntityAttackStats WeaponAttackStats { get; private set; }
    //[SerializeField] private CombatManager _combatManager;
    public Facing Facing { get; private set; }
    public Inventories Inventories { get; private set; }
    [field: SerializeField] private int _inventoryIdx { get; set; }
    private Inventory _inventory { get; set; }

    public Attackable Attackable { get; private set; }

    public List<Attackable> Friendlies { get; set; } = new List<Attackable>();

    public bool OnCooldown { get; private set; }

    public bool HasInventory { get => _inventory != null; }

    private List<Attackable> _attackablesInRange = new List<Attackable>();

    private void Awake()
    {
        QuadtreeController = GetComponentInParent<QuadtreeController>();
        Transform = transform;
        Attackable = GetComponent<Attackable>();
        Facing = GetComponent<Facing>();
        Inventories = GetComponent<Inventories>();
    }

    private void Start()
    {
        _inventory = Inventories.inventories[_inventoryIdx];
    }

    //private void OnEnable()
    //{
    //    //if (HasInventory)
    //    //{
    //    //    Inventory.ChangedSelectedItem += UpdateAttackStats;
    //    //}
    //}

    private IEnumerator DoCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnCooldown = false;
    }

    // TODO: pass in attack as decided by the AttackController
    public void TryMakeAttack()
    {
        if (_inventory.Selected == null)
        {
            AttackStats = BaseAttackStats;
        }
        else if (_inventory.Selected.WeaponComponent == null)
        {
            AttackStats = BaseAttackStats;
        }
        else
        {
            WeaponAttackStats = _inventory.Selected.WeaponComponent.AttackStats;
            AttackStats = BaseAttackStats + WeaponAttackStats;  // todo: maybe needless instantiation here..
        }
        //Debug.Log(Facing.Direction);
        Attack attack = new Attack(Attackable, Transform.position, Facing.Direction, AttackStats.BaseAttackRange, AttackStats.BaseAttackArcAngle, AttackStats.BaseAttackDamage, AttackStats.BaseKnockback);
        //Debug.Log("active attacker invoked OnAttack!");

        // invoke any item-associated attack effects
        // TODO Decouple from ActiveAttacker class? Projectile component takes in info including gameObject, origin position, direction.
        if (_inventory.Selected)
        {
            _inventory.Selected.OnAttackWith?.Invoke(attack);
        }

        if (!OnCooldown)
        {
            _attackablesInRange.Clear();
            QuadtreeController.GetComponentInBounds<Attackable>(attack.Bounds, _attackablesInRange);
            foreach (Attackable attackable in _attackablesInRange)
            {
                attackable.ChangeColour(Color.cyan, 0.2f); // debug code
                if (attackable.gameObject == gameObject || Friendlies.Contains(attackable))
                {
                    continue;
                }
                if (CheckBoundsInAttackArc(attack, attackable.Bounds))
                {
                    Debug.Log("Active attacked entity!");
                    attackable.TakeAttack(attack);
                }
            }


            OnAttack?.Invoke(attack);

            //_combatManager.SendAttack(attack);  // replace with combatManager listening to OnAttack

            OnCooldown = true;
            StartCoroutine(DoCooldown(1 / (float)AttackStats.BaseAttackSpeed));
        }
    }

    // TODO: Move into different class, and make it work for general attacks
    private bool CheckBoundsInAttackArc(Attack attack, Bounds bounds)
    {
        // debugging method
        void DrawAttackArc(Vector3 origin, float radius, float leadingAngle, float trailingAngle, Vector3 displacement)
        {
            //Debug.Log($"{origin}, {radius}, {targetAngle}, {leadingAngle}, {trailingAngle}");

            Debug.DrawLine(origin, origin + new Vector3(Mathf.Cos(leadingAngle), Mathf.Sin(leadingAngle)) * radius, Color.green, 0.2f, false);
            Debug.DrawLine(origin, origin + new Vector3(Mathf.Cos(trailingAngle), Mathf.Sin(trailingAngle)) * radius, Color.red, 0.2f, false);
            //Debug.DrawLine(origin, origin + displacement, Color.white, 0.2f, false);
        }

        // TODO: check for when inside of the entity.


        if (Vector2.SqrMagnitude((Vector2)bounds.ClosestPoint(attack.Origin) - attack.Origin) > attack.Radius * attack.Radius)
        {
            return false;
        }

        //if (InAttackArcSimple(attack, bounds.center))
        //{
        //    return true;
        //}

        var attackDirectionAngle = new Angle(Mathf.Atan2(attack.Direction.y, attack.Direction.x));
        var leadingAngleOfAttack = new Angle(attackDirectionAngle.Value + attack.Angle / 2f);
        var trailingAngleOfAttack = new Angle(attackDirectionAngle.Value - attack.Angle / 2f);

        var tl = new Vector2(bounds.min.x, bounds.min.y + bounds.size.y);
        var tr = (Vector2)bounds.max;
        var br = new Vector2(bounds.max.x, bounds.max.y - bounds.size.y);
        var bl = (Vector2)bounds.min;
        Vector2 intercept1, intercept2;  // TODO

        Vector2 tlDisp, trDisp, brDisp, blDisp;
        tlDisp = tl - attack.Origin;
        trDisp = tr - attack.Origin;
        brDisp = br - attack.Origin;
        blDisp = bl - attack.Origin;

        var disps = new Vector2[] { tlDisp, trDisp, brDisp, blDisp };

        //var angles = new Angle[4];

        //for (int i = 0; i < 4; i++)
        //{
        //    angles[i] = new Angle(Mathf.Atan2(disps[i].y, disps[i].x));
        //}

        Angle largestAngle, smallestAngle;
        largestAngle = smallestAngle = new Angle(Mathf.Atan2(disps[0].y, disps[0].x));

        for (int i = 1; i < 4; i++)
        {
            var angle = new Angle(Mathf.Atan2(disps[i].y, disps[i].x));
            if (angle > largestAngle)
            {
                largestAngle = angle;
            }
            if (angle < smallestAngle)
            {
                smallestAngle = angle;
            }
        }

        //Debug.Log($"leading angle: {leadingAngleOfAttack.Value * Mathf.Rad2Deg}, trailing angle: {trailingAngleOfAttack.Value * Mathf.Rad2Deg}\nlargest angle of bounds: {largestAngle.Value * Mathf.Rad2Deg}, smallest angle of bounds: {smallestAngle.Value * Mathf.Rad2Deg}");

        DrawAttackArc(attack.Origin, attack.Radius, leadingAngleOfAttack.Value, trailingAngleOfAttack.Value, attack.Origin);

        return !(leadingAngleOfAttack < smallestAngle || trailingAngleOfAttack > largestAngle);
    }

    private void DrawBounds(Bounds bounds, Color colour)
    {
        var tl = new Vector2(bounds.min.x, bounds.min.y + bounds.size.y);
        var tr = bounds.max;
        var br = new Vector2(bounds.max.x, bounds.max.y - bounds.size.y);
        var bl = bounds.min;

        Debug.DrawLine(tl, tr, colour, 0.2f);
        Debug.DrawLine(tr, br, colour, 0.2f);
        Debug.DrawLine(br, bl, colour, 0.2f);
        Debug.DrawLine(bl, tl, colour, 0.2f);
    }

    //private void Update()
    //{
    //    if (Inventory.Selected != null)
    //    {
    //        WeaponAttackStats = Inventory.Selected.WeaponComponent.AttackStats;
    //        AttackStats = BaseAttackStats + WeaponAttackStats;
    //    }
    //    else
    //    {
    //        AttackStats = BaseAttackStats;
    //    }
    //}

    //public void UpdateAttackStats(Item newHeldItem)
    //{
    //    if (newHeldItem == null)
    //    {
    //        AttackStats = BaseAttackStats;
    //        return;
    //    }
    //    WeaponAttackStats = newHeldItem.WeaponComponent.AttackStats;
    //    AttackStats = BaseAttackStats + WeaponAttackStats;  // todo: maybe needless instantiation here..
    //}

    //private void OnDisable()
    //{
    //    if (HasInventory)
    //    {
    //        Inventory.ChangedSelectedItem -= UpdateAttackStats;
    //    }
    //}
}
