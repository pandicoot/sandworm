using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool Component", menuName = "Scriptable Objects/Item Components/Tool")]
public class ToolComponent : ItemComponent, IEquatable<ToolComponent>
{
    private ToolComponent _originalData { get; set; }

    [field: SerializeField] public CarverHead Tool { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }

    private Item _item;
    public override Item Item { get => _item; set => _item = value; }

    //public ToolComponent(ToolComponentData data)
    //{
    //    OriginalData = data;

    //    Tool = (CarverHead)OriginalData.Tool.Clone();
    //    Speed = OriginalData.Speed;
    //}

    public override object Clone()
    {
        var newComponent = ScriptableObject.CreateInstance<ToolComponent>();
        newComponent._originalData = this;
        newComponent.Tool = Tool;
        newComponent.Speed = Speed;

        return newComponent;
    }

    public override bool Equals(object other) => this.Equals(other as ToolComponent);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Tool.GetHashCode());
        hash.Add(Speed);
        return hash.ToHashCode();
    }

    public bool Equals(ToolComponent other)
    {
        if (other == null)
        {
            return false;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        return Tool == other.Tool && Speed == other.Speed;  
    }

    public static bool operator ==(ToolComponent lhs, ToolComponent rhs)
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

    public static bool operator !=(ToolComponent lhs, ToolComponent rhs) => !(lhs == rhs);
}
