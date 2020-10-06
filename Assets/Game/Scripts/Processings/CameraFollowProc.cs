using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class CameraFollowProc : ProcessingBase, ICustomUpdate
{
    Group CamTargetGroup = Group.Create(new ComponentsList<CameraFollowCmp>());

    public void CustomUpdate()
    {
        foreach (int entity in CamTargetGroup)
        {
            //Storage.GetComponent<>(entity)
        }
    }
}
