using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// All classes that can instantiate entity objects should implement this interface (handles signalling to Entity Manager).
/// </summary>
public interface IInstantiateEntity 
{
    public event Action<GameObject> Instantiated;
}
