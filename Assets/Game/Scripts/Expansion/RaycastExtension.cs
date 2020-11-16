using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RaycastExtension
{
    public static RaycastHit[] BoxRaycast(Transform position, AttackZone attackZone, float rotationOffset, LayerMask layerMask)
    {
        RaycastHit[] castInfos;

        Quaternion rotation = Quaternion.Euler(0, 0, -attackZone.angleOffset + rotationOffset);
        Vector3 castPosition = position.position + (position.right * attackZone.distance).RotateHowVector2(-rotation.eulerAngles.z);

        castInfos = Physics.BoxCastAll(castPosition, attackZone.cubeSize / 2, Vector3.forward, rotation, 5, layerMask);

        return castInfos;
    }

    public static RaycastHit[] CircleRaycast(Transform position, float blastRadius, LayerMask layerMask)
    {
        Vector3 point1 = position.position + new Vector3(0, 0, 5);
        Vector3 point2 = position.position + new Vector3(0, 0, -5);

        RaycastHit[]  castInfos = Physics.CapsuleCastAll(point1, point2, blastRadius, new Vector3(0, 0, -1), 1, layerMask);

        return castInfos;
    }

}
