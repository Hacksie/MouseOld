using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void Randomize<T>(this List<T> list)
    {
        T temp;

        for (int i = 0; i < list.Count; i++)
        {
            int r = UnityEngine.Random.Range(i, list.Count);
            temp = list[r];
            list[r] = list[i];
            list[i] = temp;
        }
    }

}