using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private EntityManager _entityManager { get; set; }

    #region Component lists
    private List<Attackable> _attackables = new List<Attackable>();
    private List<CharacterMovement> _attackableMovements = new List<CharacterMovement>();

    private List<PassiveAttacker> _passiveAttackers = new List<PassiveAttacker>();
    private List<CharacterMovement> _passiveAttackerMovements = new List<CharacterMovement>();

    private List<ActiveAttacker> _activeAttackers = new List<ActiveAttacker>();
    #endregion

    private QuadtreeController QuadtreeController { get; set; }

    private Queue<Attack> _attacks = new Queue<Attack>();


    private bool _wasDisabled = false;

    private void Awake()
    {
        _entityManager = GetComponent<EntityManager>();
        QuadtreeController = GetComponent<QuadtreeController>();
    }

    void OnEnable()
    {
        //_activeAttackers.ForEach(atkr => atkr.OnAttack += AddAttackToQueue);
        _entityManager.AddedNewEntity += _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity += _entityManager_RemovedEntity;

        if (_wasDisabled)
        {
            _activeAttackers.ForEach(atkr => atkr.OnAttack += AddAttackToQueue);
        }
    }

    private void Start()
    {
        _attackables = _entityManager.GetComponentsInEntities<Attackable>();
        _attackables.ForEach(x => _attackableMovements.Add(x.GetComponent<CharacterMovement>()));  // what if attackable, but no movmement?!

        _passiveAttackers = _entityManager.GetComponentsInEntities<PassiveAttacker>();
        _passiveAttackers.ForEach(x => _passiveAttackerMovements.Add(x.GetComponent<CharacterMovement>()));

        _activeAttackers = _entityManager.GetComponentsInEntities<ActiveAttacker>();

        _activeAttackers.ForEach(atkr => atkr.OnAttack += AddAttackToQueue);
    }

    private void _entityManager_AddedNewEntity(GameObject obj)
    {
        if (obj.TryGetComponent<Attackable>(out Attackable atkable))
        {
            _attackables.Add(atkable);
            if (obj.TryGetComponent<CharacterMovement>(out CharacterMovement move))
            {
                _attackableMovements.Add(move);
            }
        }

        if (obj.TryGetComponent<PassiveAttacker>(out PassiveAttacker atkr))
        {
            _passiveAttackers.Add(atkr);
            if (obj.TryGetComponent<CharacterMovement>(out CharacterMovement move))
            {
                _passiveAttackerMovements.Add(move);
            }
        }

        if (obj.TryGetComponent<ActiveAttacker>(out ActiveAttacker activeatkr))
        {
            _activeAttackers.Add(activeatkr);
        }
    }

    private void _entityManager_RemovedEntity(GameObject obj)
    {
        // Assumes that the component lists are correct, for performance
        if (obj.TryGetComponent<Attackable>(out Attackable atkable))
        {
            _attackables.Remove(atkable);
            if (obj.TryGetComponent<CharacterMovement>(out CharacterMovement move))
            {
                _attackableMovements.Remove(move);
            }
        }

        if (obj.TryGetComponent<PassiveAttacker>(out PassiveAttacker atkr))
        {
            _passiveAttackers.Remove(atkr);
            if (obj.TryGetComponent<CharacterMovement>(out CharacterMovement move))
            {
                _passiveAttackerMovements.Remove(move);
            }
        }

        if (obj.TryGetComponent<ActiveAttacker>(out ActiveAttacker activeatkr))
        {
            _activeAttackers.Remove(activeatkr);
        }
    }

    void OnDisable()
    {
        _activeAttackers.ForEach(atkr => atkr.OnAttack -= AddAttackToQueue);
        _entityManager.AddedNewEntity -= _entityManager_AddedNewEntity;
        _entityManager.RemovedEntity -= _entityManager_RemovedEntity;

        _wasDisabled = true;
    }

    public void AddAttackToQueue(Attack attack)
    {
        _attacks.Enqueue(attack);
        Debug.Log(attack.Attacker + "attacked");
    }

    #region Utils
    public int CheckPointPositionRelativeToLine(Vector2 point, Vector2 origin, Vector2 dir)
    {
        int res = 0;
        switch (point.y - origin.y - (dir.y / dir.x) * (point.x - origin.x))
        {
            case > 0:
                res = 1;
                break;
            case < 0:
                res = -1;
                break;
        }
        return res;
    }

    public bool CheckRayIntersectsWithBounds(Bounds bounds, Vector2 origin, Vector2 dir, float length = Mathf.Infinity)
    {
        var tl = new Vector2(bounds.min.x, bounds.min.y + bounds.size.y);
        var tr = bounds.max;
        var br = new Vector2(bounds.max.x, bounds.max.y - bounds.size.y);
        var bl = bounds.min;

        var points = new Vector2[4] { tl, tr, br, bl };

        var rayBounds = new Bounds(origin + (length / 2f) * dir, dir * length);

        if (Array.TrueForAll<Vector2>(points, x => !rayBounds.Contains(x)))  // unnecessary
        {
            return false;
        }

        return (CheckPointPositionRelativeToLine(tl, origin, dir) == CheckPointPositionRelativeToLine(tr, origin, dir)
            && CheckPointPositionRelativeToLine(tr, origin, dir) == CheckPointPositionRelativeToLine(br, origin, dir)
            && CheckPointPositionRelativeToLine(br, origin, dir) == CheckPointPositionRelativeToLine(bl, origin, dir));
    }

    public bool CheckPtInCircle(Vector2 origin, float radius, Vector2 pt)
    {
        return Vector2.SqrMagnitude(pt - origin) <= radius * radius;
    }

    public int GetQuadrant(Vector2 localPt)
    {
        if (localPt.x > 0 && localPt.y > 0)
        {
            return 0;
        }
        if (localPt.x < 0 && localPt.y > 0)
        {
            return 1;
        }
        if (localPt.x < 0 && localPt.y < 0)
        {
            return 2;
        }
        if (localPt.x > 0 && localPt.y < 0)
        {
            return 3;
        }
        return -1;
    }

    public List<Vector2> GetInterceptOfBoundsWithCircle(Vector2 origin, float radius, Bounds bounds, Vector2 tl, Vector2 tr, Vector2 br, Vector2 bl)
    {
        var intersections = new List<Vector2>();

        if (Vector2.SqrMagnitude((Vector2)bounds.ClosestPoint(origin) - origin) > radius * radius)
        {
            return null;
        }

        // operate in local space rt centre of circle


        (Vector2, Vector2) topEdge, rightEdge, bottomEdge, leftEdge;
        //topEdge = rightEdge = bottomEdge = leftEdge = (Vector2.negativeInfinity, Vector2.zero);
        topEdge = (tl, tr - tl);
        rightEdge = (tr, br - tr);
        bottomEdge = (br, bl - br);
        leftEdge = (bl, tl - bl);

        var tlDisp = tl - origin;
        var trDisp = tr - origin;
        var brDisp = br - origin;
        var blDisp = bl - origin;

        //if (CheckPtInCircle(origin, radius, tl) != CheckPtInCircle(origin, radius, tr))
        //{
        //    topEdge = (tl, tr - tl);
        //}
        //if (CheckPtInCircle(origin, radius, tr) != CheckPtInCircle(origin, radius, br))
        //{
        //    rightEdge = (tr, br - tr);
        //}
        //if (CheckPtInCircle(origin, radius, br) != CheckPtInCircle(origin, radius, bl))
        //{
        //    bottomEdge = (br, bl - br);
        //}
        //if (CheckPtInCircle(origin, radius, bl) != CheckPtInCircle(origin, radius, tl))
        //{
        //    leftEdge = (bl, tl - bl);
        //}

        bool[] quadrantsCovered = new bool[4];
        // top edge
        quadrantsCovered[GetQuadrant(tlDisp)] = true;
        quadrantsCovered[GetQuadrant(trDisp)] = true;

        var topXSq = radius * radius - tlDisp.y * tlDisp.y;
        if (topXSq >= 0)
        {
            var x = Mathf.Sqrt(topXSq);
            var xNeg = -x;

            // todo
        }
        Array.Clear(quadrantsCovered, 0, 4);
        // bottom edge

        // left edge

        // right edge


        return null;
    }
    #endregion

    private bool CheckBoundsInAttackArc(Attack attack, Bounds bounds)
    {
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

    private bool InAttackArcSimple(Attack attack, Vector2 pos)
    {
        var displacement = pos - attack.Origin;

        var targetAngle = new Angle(Mathf.Atan2(displacement.y, displacement.x));

        var attackDirectionAngle = new Angle(Mathf.Atan2(attack.Direction.y, attack.Direction.x));

        var leadingAngle = new Angle((attackDirectionAngle.Value + attack.Angle / 2f));
        var trailingAngle = new Angle((attackDirectionAngle.Value - attack.Angle / 2f));

        DrawAttackArc(attack.Origin, attack.Radius, leadingAngle.Value, trailingAngle.Value, displacement);

        //var LeadLessX = ((Mathf.PI * 2 - targetAngle) + leadingAngle) % (2 * Mathf.PI);
        //var XLessTrail = ((Mathf.PI * 2 - trailingAngle) + targetAngle) % (2 * Mathf.PI);
        //Debug.Log($"target angle: {targetAngle * Mathf.Rad2Deg}, leading angle: {leadingAngle * Mathf.Rad2Deg}, trailing angle: {trailingAngle * Mathf.Rad2Deg}\n" +
        //    $"Lead - x: {LeadLessX * Mathf.Rad2Deg}, x - trail: {XLessTrail * Mathf.Rad2Deg}");

        return Vector2.SqrMagnitude(displacement) <= attack.Radius * attack.Radius
            && (leadingAngle > targetAngle)
            && (targetAngle > trailingAngle);
    }

    //private bool InAttackArcSimple(Attack attack, Vector2 pos)
    //{
    //    var displacement = pos - attack.Origin;

    //    var targetAngle = (Mathf.Atan2(displacement.y, displacement.x) + Mathf.PI * 2) % (2*Mathf.PI);

    //    var attackDirectionAngle = (Mathf.Atan2(attack.Direction.y, attack.Direction.x) + Mathf.PI * 2) % (2*Mathf.PI);

    //    var leadingAngle = ((attackDirectionAngle + attack.Angle / 2f) + Mathf.PI * 2) % (2*Mathf.PI);
    //    var trailingAngle = ((attackDirectionAngle - attack.Angle / 2f) + Mathf.PI * 2) % (2 * Mathf.PI);

    //    DrawAttackArc(attack.Origin, attack.Radius, leadingAngle, trailingAngle, displacement, targetAngle);

    //    var LeadLessX = ((Mathf.PI * 2 - targetAngle) + leadingAngle) % (2 * Mathf.PI);
    //    var XLessTrail = ((Mathf.PI * 2 - trailingAngle) + targetAngle) % (2 * Mathf.PI);
    //    Debug.Log($"target angle: {targetAngle * Mathf.Rad2Deg}, leading angle: {leadingAngle * Mathf.Rad2Deg}, trailing angle: {trailingAngle * Mathf.Rad2Deg}\n" +
    //        $"Lead - x: {LeadLessX * Mathf.Rad2Deg}, x - trail: {XLessTrail * Mathf.Rad2Deg}");

    //    return Vector2.SqrMagnitude(displacement) <= attack.Radius * attack.Radius
    //        && (LeadLessX > 0 && LeadLessX < attack.Angle)
    //        && (XLessTrail > 0 && XLessTrail < attack.Angle);
    //}

    //private void Update()
    //{
    //    var attackablesInRange = new List<Attackable>();
    //    // active attacks
    //    while (_attacks.Count > 0)
    //    {
    //        attackablesInRange.Clear();

    //        var attack = _attacks.Dequeue();
    //        var attackBounds = attack.Bounds;
    //        //DrawBounds(attackBounds);
    //        //foreach (var attackable in _attackables)
    //        //{
    //        //    if (attack.Attacker.gameObject == attackable.gameObject)
    //        //    {
    //        //        continue;
    //        //    }

    //        //    if (InAttackArc(attack, attackable.transform.position))
    //        //    {
    //        //        attackable.TakeAttack(attack);
    //        //    }
    //        //}
    //        DrawBounds(attackBounds, Color.yellow);
    //        QuadtreeController.GetAttackablesInBounds(attackBounds, attackablesInRange);  // have to modify to work with bounds min/maxima, rather than just transform positions
    //        foreach (Attackable attackable in attackablesInRange)
    //        {
    //            attackable.ChangeColour(Color.cyan, 0.2f);
    //            if (attackable.gameObject == attack.Attacker.gameObject)
    //            { 
    //                continue;
    //            }
    //            if (CheckBoundsInAttackArc(attack, attackable.Bounds))
    //            {
    //                Debug.Log("Active attacked entity!");
    //                attackable.TakeAttack(attack);
    //            }
    //        }
            
    //    }

    //    // passive attacks
    //    // O(n^2)
    //    //for (int i = 0; i < _attackables.Count; i++)
    //    //{
    //    //    for (int j = 0; j < _passiveAttackers.Count; j++)
    //    //    {
    //    //        var attack = _passiveAttackers[j].Attack;
    //    //        if (attack.Attacker.gameObject == _attackables[i].gameObject)
    //    //        {
    //    //            continue;
    //    //        }

    //    //        if (_passiveAttackers[j].Bounds.Intersects(_attackables[i].Bounds))
    //    //        {
    //    //            _attackables[i].TakeAttack(new Attack(attack.Attacker, attack.Origin, (_passiveAttackerMovements[j].Velocity - _attackableMovements[i].Velocity).normalized, 0f, 0f, attack.Damage, attack.Knockback));
    //    //        }
    //    //    }
    //    //}
        
    //    /*for (int i = 0; i < _passiveAttackers.Count; i++)
    //    {
    //        attackablesInRange.Clear();

    //        var attack = _passiveAttackers[i].Attack;
    //        var bounds = _passiveAttackers[i].Bounds;
    //        QuadtreeController.GetAttackablesInBounds(bounds, attackablesInRange);

    //        foreach (Attackable attackable in attackablesInRange)
    //        {
    //            // self check
    //            if (attackable.gameObject == _passiveAttackers[i].gameObject)
    //            {
    //                continue;
    //            }

    //            if (_passiveAttackers[i].Bounds.Intersects(attackable.Bounds))
    //            {
    //                var direction = Vector2.zero;

    //                if (_passiveAttackers[i].Movement != null)
    //                {
    //                    direction = _passiveAttackers[i].Movement.Velocity.normalized;
    //                }

    //                //if (attackable.Movement == null)
    //                //{
    //                //    direction = Vector2.zero;
    //                //}
    //                //else
    //                //{
    //                //    if (_passiveAttackers[i].Movement == null)
    //                //    {
    //                //        direction = -attackable.Movement.Velocity.normalized;
    //                //    }
    //                //    else
    //                //    {
    //                //        direction = (_passiveAttackers[i].Movement.Velocity - attackable.Movement.Velocity).normalized;
    //                //    }
    //                //}

    //                attackable.TakeAttack(new Attack(attack.Attacker, attack.Origin, direction, 0f, 0f, attack.Damage, attack.Knockback));
    //            }
    //        }
    //    }*/


    //}

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

    // debugging method
    private void DrawAttackArc(Vector3 origin, float radius, float leadingAngle, float trailingAngle, Vector3 displacement)
    {
        //Debug.Log($"{origin}, {radius}, {targetAngle}, {leadingAngle}, {trailingAngle}");

        Debug.DrawLine(origin, origin + new Vector3(Mathf.Cos(leadingAngle), Mathf.Sin(leadingAngle)) * radius, Color.green, 0.2f, false);
        Debug.DrawLine(origin, origin + new Vector3(Mathf.Cos(trailingAngle), Mathf.Sin(trailingAngle)) * radius, Color.red, 0.2f, false);
        //Debug.DrawLine(origin, origin + displacement, Color.white, 0.2f, false);
    }
}
