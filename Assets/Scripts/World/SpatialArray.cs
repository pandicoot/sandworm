using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject]
public class SpatialArray<T> 
{
    // TODO: indexer property implementation
    // TODO: iterators
    [Key(0)]
    public T[] Array;
    [Key(1)]
    public readonly int Length;
    [Key(2)]
    public readonly int Height;


    public SpatialArray(int length, int height)
    {
        this.Length = length;
        this.Height = height;

        Array = new T[Length * Height];
    }

    public SpatialArray(T[] arr, int length, int height)
    {
        this.Length = length;
        this.Height = height;

        Array = (T[])arr.Clone();
    }

    public void Set(T elem, int x, int y)
    {
        int xIndex = x;
        int yIndex = Height - 1 - y;

        Debug.Assert((xIndex >= 0 && xIndex < Length), "Invalid x index");
        Debug.Assert((yIndex >= 0 && yIndex < Height), "Invalid y index");

        Array[yIndex * Length + xIndex] = elem;
    }

    public void Set(T elem, int i)
    {
        Debug.Assert((i >= 0 && i < Length * Height), "Invalid index");

        int xIndex = i % Length;
        int yIndex = Height - 1 - Mathf.FloorToInt(i / (float)Length);

        Array[yIndex * Length + xIndex] = elem;
    }

    public T Get(int x, int y)
    {
        int xIndex = x;
        int yIndex = Height - 1 - y;

        Debug.Assert((xIndex >= 0 && xIndex < Length), "Invalid x index");
        Debug.Assert((yIndex >= 0 && yIndex < Height), "Invalid y index");

        return Array[yIndex * Length + xIndex];
    }

    public T Get(int i)
    {
        Debug.Assert((i >= 0 && i < Length * Height), "Invalid index");

        int xIndex = i % Length;
        int yIndex = Height - 1 - Mathf.FloorToInt(i / (float)Length);
        return Array[yIndex * Length + xIndex];
    }

    public bool CheckInBounds(int x, int y)
    {
        return (x >= 0 && x < Length) && (y >= 0 && y < Height);
    }
    public bool CheckInBounds(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.x < Length) && (pos.y >= 0 && pos.y < Height);
    }
}
