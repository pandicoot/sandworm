using UnityEngine;

[CreateAssetMenu(fileName = "New Destruct Component", menuName = "Scriptable Objects/Item Components/Destruct")]
public class DestructComponent : ToolComponent
{
    public override Item Item
    {
        get => base.Item;
        set
        {
            _item = value;
        }
    }

    public override object Clone()
    {
        var newComponent = ScriptableObject.CreateInstance<DestructComponent>();
        newComponent._originalData = this;
        newComponent.Tool = Tool;
        newComponent.Speed = Speed;
        newComponent.ConsumeUponUse = ConsumeUponUse;

        return newComponent;
    }

    public bool Equals(DestructComponent other)
    {
        if (other == null)
        {
            return false;
        }
        return base.Equals(other);
    }
    public override bool Equals(object other) => Equals(other as DestructComponent);
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator ==(DestructComponent lhs, DestructComponent rhs)
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
    public static bool operator !=(DestructComponent lhs, DestructComponent rhs) => !(lhs == rhs);
}
