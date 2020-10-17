using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class DelayDestroyProc : ProcessingBase, ICustomUpdate
{
    Group Group = Group.Create(new ComponentsList<DelayedDestroyCmp>());

    public void CustomUpdate()
    {
        foreach (int entity in Group)
        {
            GameObject.Destroy(EntityBase.GetEntity(entity).gameObject, Storage.GetComponent<DelayedDestroyCmp>(entity).time);
        }
    }

    
}
