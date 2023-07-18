using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject]
public class Structure 
{
    [Key(0)]
    public SpatialArray<Tiles> TileMap { get; private set; }
}
