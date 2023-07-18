using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public static class DisplayTilesAsText
{
    public static string DisplayAsText(Tiles[] world, int length, int height)
    {
        StringBuilder display = new StringBuilder();

        for (int y = 0; y < height; y++)
        {
            display.Append("[");
            for (int x = 0; x < length; x++)
            {
                int tile = (int)world[y * length + x];
                display.AppendFormat($" {tile}");
            }
            display.AppendLine(" ]");
        }

        return display.ToString();
    }

    //public static string DisplayAsText(ArraySegment<Tiles> world, int length, int height)
    //{
    //    StringBuilder display = new StringBuilder();

    //    for (int y = 0; y < height; y++)
    //    {
    //        display.Append("[");
    //        for (int x = 0; x < length; x++)
    //        {
    //            int tile = (int)world[y * length + x];
    //            display.AppendFormat($" {tile}");
    //        }
    //        display.AppendLine(" ]");
    //    }

    //    return display.ToString();
    //}

    public static string DisplayAsText(SpatialArray<Tiles> world, int length, int height)
    {
        StringBuilder display = new StringBuilder();

        for (int y = height - 1; y >= 0; y--)
        {
            display.Append("[");
            for (int x = 0; x < length; x++)
            {
                int tile = (int)world.Get(x, y);
                display.AppendFormat($" {tile}");
            }
            display.AppendLine(" ]");
        }

        return display.ToString();
    }
}
