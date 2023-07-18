using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour  // Static or Instanced class singleton instead?
{
    public static HashSet<Tiles> Air = new HashSet<Tiles> { Tiles.Air };
    public static HashSet<Tiles> AllTiles = new HashSet<Tiles> { Tiles.Air, Tiles.Grass, Tiles.Dirt, Tiles.Stone, Tiles.Sand, Tiles.Darkstone, Tiles.WoodenPlanks, Tiles.StoneBricks, Tiles.Granite, Tiles.Marble, Tiles.Mud, Tiles.Mudstone };
    public static HashSet<Tiles> AllPhysicalTiles = new HashSet<Tiles> { Tiles.Grass, Tiles.Dirt, Tiles.Stone, Tiles.Sand, Tiles.Darkstone, Tiles.WoodenPlanks, Tiles.StoneBricks, Tiles.Granite, Tiles.Marble, Tiles.Mud, Tiles.Mudstone };

    public static List<Tiles> AllTilesList = new List<Tiles> { Tiles.Air, Tiles.Grass, Tiles.Dirt, Tiles.Stone, Tiles.Sand, Tiles.Darkstone, Tiles.WoodenPlanks, Tiles.StoneBricks, Tiles.Granite, Tiles.Marble, Tiles.Mud, Tiles.Mudstone };
    public static List<Tiles> AllPhysicalTilesList = new List<Tiles> { Tiles.Grass, Tiles.Dirt, Tiles.Stone, Tiles.Sand, Tiles.Darkstone, Tiles.WoodenPlanks, Tiles.StoneBricks, Tiles.Granite, Tiles.Marble, Tiles.Mud, Tiles.Mudstone };

    //[System.Serializable]
    //class Tile
    //{
    //    public Vector2 uv;
    //    // more to be added later

    //    public Tile(Vector2 uv)
    //    {
    //        this.uv = uv;
    //    }
    //}

    //private const int s_buffMax = 255;
    //private Tile[] _tiles = new Tile[s_buffMax];   // buffer max
    //private int _index;

    //public void CreateNewTile(Vector2 uv)
    //{
    //    _tiles[_index++] = new Tile(uv);
    //}

    //public void DeleteTile(ushort index)
    //{
    //    //_tiles[index] = null;
    //    // shift everything back
    //    for (int j = index + 1; j < s_buffMax; j++)  // modifying the thing we are iterating over (!)
    //    {
    //        _tiles[j - 1] = _tiles[j];
    //    }
    //    _tiles[s_buffMax - 1] = null;
    //}

    //public Vector2 GetUV(ushort index)
    //{
    //    return _tiles[index].uv;
    //}
}
