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
            CameraFollowCmp cameraFollowCmp = Storage.GetComponent<CameraFollowCmp>(entity);
            Transform target = cameraFollowCmp.transform;

            Vector3 CamPos = new Vector3();

            CamPos = Vector3.Lerp(Camera.main.transform.position, 
                new Vector3(target.position.x + cameraFollowCmp.Offset.x, target.position.y + cameraFollowCmp.Offset.y, Camera.main.transform.position.z), 
                cameraFollowCmp.learp);

            //Input.mousePosition.y
            //Camera.main.pixelWidth;

            CamPos += new Vector3(
                ((Input.mousePosition.x - (Camera.main.pixelWidth  / 2)) / Camera.main.pixelWidth ) * cameraFollowCmp.CursorOffset.x, 
                ((Input.mousePosition.y - (Camera.main.pixelHeight / 2)) / Camera.main.pixelHeight) * cameraFollowCmp.CursorOffset.y,
                0);


            Camera.main.transform.position = CamPos;

            cameraFollowCmp.AlphaMask.SetVector("_FadeOrigin", new Vector4(target.position.x + cameraFollowCmp.Offset.x, target.position.y + cameraFollowCmp.Offset.y, 0, 1));
            break;
        }
    }


}
