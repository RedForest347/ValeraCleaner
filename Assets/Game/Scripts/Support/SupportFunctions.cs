using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SupportFunction
{
    /// <summary>
    /// убирает элементы с одинаковыми attachedRigidbody, удаляет элементы с null attachedRigidbody, т.е.
    /// убирает остаются элементы с уникальными attachedRigidbody, которые не равны null
    /// </summary>
    public static RaycastHit[] CreateCorrectRaycastHits(RaycastHit[] array)
    {
        Dictionary<Rigidbody, RaycastHit> Some = new Dictionary<Rigidbody, RaycastHit>();
        Rigidbody rigidbody;
        for (int i = 0; i < array.Length; i++)
        {
            rigidbody = array[i].collider.attachedRigidbody;
            if (rigidbody != null)
            {
                if (!Some.ContainsKey(rigidbody))
                {
                    Some.Add(rigidbody, array[i]);
                }
            }
        }

        if (Some.Keys.Count == array.Length)
            return array;

        array = new RaycastHit[Some.Keys.Count];

        int index = 0;
        foreach (RaycastHit hit in Some.Values)
        {
            array[index++] = hit;
        }

        return array;
    }
}
