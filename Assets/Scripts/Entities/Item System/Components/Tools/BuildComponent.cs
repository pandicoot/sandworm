using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Build Component", menuName = "Scriptable Objects/Item Components/Build")]
public class BuildComponent : ToolComponent, IEquatable<BuildComponent>
{
    public override Item Item {
        get => base.Item;
        set
        {
            _item = value;
        }
    }

    public override void OnBuildWith(int n)
    {
        if (ConsumeUponUse)
        {
            OnRequestRemoveItem(n);
            //Item.RequestRemove.Invoke(n);
        }
    }

    public override object Clone()
    {
        var newComponent = ScriptableObject.CreateInstance<BuildComponent>();
        newComponent._originalData = this;
        newComponent.Tool = Tool;
        newComponent.Speed = Speed;
        newComponent.ConsumeUponUse = ConsumeUponUse;

        return newComponent;
    }

    public bool Equals(BuildComponent other)
    {
        if (other == null)
        {
            return false;
        }
        return base.Equals(other);
    }
    public override bool Equals(object other) => Equals(other as BuildComponent);
    public override int GetHashCode()
    {
        return base.GetHashCode();  
    }
    public static bool operator ==(BuildComponent lhs, BuildComponent rhs)
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
    public static bool operator !=(BuildComponent lhs, BuildComponent rhs) => !(lhs == rhs);
}
