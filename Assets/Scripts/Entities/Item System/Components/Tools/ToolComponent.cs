using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ToolComponent : ItemComponent, IEquatable<ToolComponent>
{
    protected ToolComponent _originalData { get; set; }

    [field: SerializeField] public CarverHead Tool { get; protected set; }
    [field: SerializeField] public float Speed { get; protected set; }
    [field: SerializeField] public bool ConsumeUponUse { get; protected set; }

    //public override Item Item { get => _item;
    //    set
    //    {
    //        //if (_item)
    //        //{
    //        //    _item.OnBuildWith -= OnBuildWith;
    //        //    _item.OnDestructWith -= OnDestructWith;
    //        //}
    //        //if (value)
    //        //{
    //        //    value.OnBuildWith += OnBuildWith;
    //        //    value.OnDestructWith += OnDestructWith;
    //        //}
    //        _item = value;
    //    }
    //}

    //public ToolComponent(ToolComponentData data)
    //{
    //    OriginalData = data;

    //    Tool = (CarverHead)OriginalData.Tool.Clone();
    //    Speed = OriginalData.Speed;
    //}

    public override bool Equals(object other) => this.Equals(other as ToolComponent);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Tool.GetHashCode());
        hash.Add(Speed);
        hash.Add(ConsumeUponUse);
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

        return Tool == other.Tool && Speed == other.Speed && ConsumeUponUse == other.ConsumeUponUse;  
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
