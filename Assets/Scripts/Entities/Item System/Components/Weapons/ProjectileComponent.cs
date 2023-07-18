using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Component", menuName = "Scriptable Objects/Item Components/Projectile")]
public class ProjectileComponent : ItemComponent, IInstantiateEntity, IEquatable<ProjectileComponent>
{
    private Item _item;
    public override Item Item
    {
        get => _item;
        set
        {
            if (_item)
            {
                _item.OnAttackWith -= FireProjectile;
            }

            if (value)
            {
                value.OnAttackWith += FireProjectile;
            }
            _item = value;
            
        }
    }

    private ProjectileComponent _originalData { get; set; }

    [field: SerializeField] public ProjectileStats ProjectileStats { get; private set; }
    [field: SerializeField] public GameObject Projectile { get; private set; }

    public event Action<GameObject> Instantiated;

    public bool OnCooldown { get; private set; }

    // TODO: isn't listening to ActiveAttacker?
    public void FireProjectile(Attack attack)
    {
        if (!OnCooldown)
        {
            var origin = attack.Origin;
            var direction = attack.Direction;

            var projectile = Instantiate(Projectile, origin, Quaternion.identity).GetComponent<Projectile>();
            var movementController = projectile.GetComponent<MovementController>();
            projectile.ProjectileStats = ProjectileStats;
            projectile.Owner = attack.Attacker.gameObject;

            movementController.AddImpulse(ProjectileStats.BaseImpulse * direction);
            Instantiated?.Invoke(projectile.gameObject);

            OnCooldown = true;
            attack.Attacker.StartCoroutine(DoCooldown(1 / ProjectileStats.BaseFireRate));
        }
    }

    private IEnumerator DoCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnCooldown = false;
    }

    public override object Clone()
    {
        var newComponent = ScriptableObject.CreateInstance<ProjectileComponent>();
        newComponent._originalData = this;
        newComponent.ProjectileStats = (ProjectileStats)ProjectileStats.Clone();
        newComponent.Projectile = Projectile;

        //Debug.Assert(Item != null, "Parent item not set!");
        //Item.OnAttackWith += newComponent.FireProjectile;

        return newComponent;
    }

    private void OnValidate()
    {
        if (Projectile != null)
        {
            Debug.Assert(Projectile.GetComponent<Movement>() != null, "No movement component attached to projectile.");
            Debug.Assert(Projectile.GetComponent<MovementController>() != null, "No movement controller component attached to projectile.");
        }
    }

    public override bool Equals(object other) => this.Equals(other as ProjectileComponent);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(ProjectileStats.GetHashCode());
        hash.Add(Projectile.GetHashCode());
        return hash.ToHashCode();
    }

    public bool Equals(ProjectileComponent other)
    {
        if (other == null)
        {
            return false;
        }

        if (this.GetType() != other.GetType()) {
            return false;
        }

        return ProjectileStats == other.ProjectileStats && Projectile == other.Projectile;  // TODO: value equality & hash code for ProjectileStats 
    }

    public static bool operator ==(ProjectileComponent lhs, ProjectileComponent rhs)
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

    public static bool operator !=(ProjectileComponent lhs, ProjectileComponent rhs) => !(lhs == rhs);

}
