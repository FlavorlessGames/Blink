using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static bool InRange(Vector3 position1, Vector3 position2, float range)
    {
        return Vector3.Distance(position1, position2) < range;
    }
    public static bool InRange(Vector3 position1, Vector3 position2, int range)
    {
        return Vector3.Distance(position1, position2) < range;
    }
    public static bool InRange(Vector3 position1, Vector3 position2, double range)
    {
        return Vector3.Distance(position1, position2) < range;
    }
    public static List<T> ListCopy<T>(List<T> list)
    {
        List<T> newList = new List<T>(); 
        foreach (T item in list)
        {
            newList.Add(item);
        }
        return newList;
    }
}