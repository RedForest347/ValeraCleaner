using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;

public static class RaycastHitExtension
{

    public static EntityBase[] CreateEntityBaseArray(this RaycastHit[] castInfos, int entitySender = -1)
    {
        RaycastHit[] _castInfos = CreateCorrectRaycastHits(castInfos);
        Debug.Log("_castInfos.Length = " + _castInfos.Length);
        int index = 0;
        EntityBase[] entityBases = new EntityBase[_castInfos.Length];

        for (int i = 0; i < _castInfos.Length; i++)
        {
            Rigidbody rb = _castInfos[i].collider.attachedRigidbody;
            if (rb != null && rb.TryGetComponent(out EntityBase target))
                if (target.entity != entitySender)
                    entityBases[index++] = target;
        }
        Debug.Log("index = " + index);
        Array.Resize(ref entityBases, index);
        return entityBases;
    }

    static RaycastHit[] CreateCorrectRaycastHits(RaycastHit[] array)
    {
        Dictionary<Rigidbody, RaycastHit> Some = new Dictionary<Rigidbody, RaycastHit>();
        Rigidbody rigidbody;

        for (int i = 0; i < array.Length; i++)
        {
            rigidbody = array[i].collider.attachedRigidbody;
            if (rigidbody != null)
                if (!Some.ContainsKey(rigidbody))
                    Some.Add(rigidbody, array[i]);
        }

        if (Some.Keys.Count == array.Length)
            return array;

        array = new RaycastHit[Some.Keys.Count];

        int index = 0;
        foreach (RaycastHit hit in Some.Values)
            array[index++] = hit;

        return array;
    }
}
