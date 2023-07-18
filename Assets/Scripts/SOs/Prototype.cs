using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prototype : ScriptableObject, ICloneable
{
    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }
}
