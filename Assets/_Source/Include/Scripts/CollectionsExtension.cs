using System.Collections.Generic;
using UnityEngine;

public static class CollectionsExtension
{
    public static T GetRandomElement<T>(this List<T> collection)
    {
        return collection[Random.Range(0, collection.Count)];
    }

    public static T GetRandomElement<T>(this T[] collection)
    {
        return collection[Random.Range(0, collection.Length)];
    }
}