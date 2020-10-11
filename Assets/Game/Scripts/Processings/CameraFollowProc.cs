using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class CameraFollowProc : ProcessingBase, ICustomFixedUpdate
{
    Group CamTargetGroup = Group.Create(new ComponentsList<CameraFollowCmp>());

    public void CustomFixedUpdate()
    {
        foreach (int entity in CamTargetGroup)
        {
            Transform target = Storage.GetComponent<CameraFollowCmp>(entity).transform;
            Camera.main.transform.position =
                Vector3.Lerp(Camera.main.transform.position,
                new Vector3(target.position.x, target.position.y, -10), Storage.GetComponent<CameraFollowCmp>(entity).learp);
            Storage.GetComponent<CameraFollowCmp>(entity).AlphaMask.SetVector("_FadeOrigin", new Vector4(target.position.x, target.position.y + 0.5f, 0, 1));
            break;
        }
    }


}
