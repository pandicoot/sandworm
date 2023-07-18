using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventChannel<T> : ScriptableObject 
{
    public UnityAction<T> OnEventRequested;

    public void RaiseEvent(T arg)
    {
        OnEventRequested?.Invoke(arg);
    }
}

[CreateAssetMenu(fileName = "New Float Event Channel", menuName = "Scriptable Objects/Event Channels/Float Event Channel")]
public class FloatEventChannel : EventChannel<float>
{
}



