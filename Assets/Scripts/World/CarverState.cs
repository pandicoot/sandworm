using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarverState : State
{
    public CarverHead CarverHead { get; private set; }
    public CarverHeadParameters CarverHeadParameters { get; private set; }
    public CarverParameters CarverParameters { get; private set; }

    public CarverState()
    {
    }

}
