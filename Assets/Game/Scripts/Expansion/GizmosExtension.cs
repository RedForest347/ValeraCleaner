using System;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions.Must;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// функции расширения Gizmos. следует использовать только в OnDrawGizmos/OnDrawGizmosSelected
/// цвет задается изменением Gizmos.color
/// </summary>
public static class GizmosExtension
{


    #region DrawZone

    /// <summary>
    /// рисует зону окружности
    /// зона рисуется по сторонам от вектора targetTransform.up + angle_offset
    /// </summary>
    /// <param name="targetTransform">трансформ, относительно которого рисуется зона</param>
    public static void DrawZone(this Gizmos gizmos, Transform targetTransform, float angle, float distance, Color color)
    {
        DrawZoneBase(targetTransform, targetTransform.up, angle, 0, distance);
    }


    public static void DrawZone(Transform targetTransform, float angle, float distance, Color color)
    {
        DrawZoneBase(targetTransform, targetTransform.up, angle, 0, distance);
    }

    public static void DrawZone(this Gizmos gizmos, Transform targetTransform, float angle, float angle_offset, float distance, Color color)
    {
        Vector3 from = Vector2Extension.Rotate(targetTransform.up, angle_offset);
        DrawZoneBase(targetTransform, from, angle, angle_offset, distance);
    }

    public static void DrawZone(Transform targetTransform, float angle, float angle_offset, float distance, Color color)
    {
        Vector3 from = Vector2Extension.Rotate(targetTransform.up, angle_offset);
        DrawZoneBase(targetTransform, from, angle, angle_offset, distance);
    }

    static void DrawZoneBase(Transform targetTransform, Vector3 from, float angle, float angle_offset, float distance)
    {
#if UNITY_EDITOR
        angle = angle / 2;
        Color temp = Handles.color;
        Handles.color = Gizmos.color;
        Handles.DrawSolidArc(targetTransform.position, Vector3.forward, from, angle, distance);
        Handles.DrawSolidArc(targetTransform.position, Vector3.forward, from, -angle, distance);
        Handles.color = Color.black;

        if (angle < 180 - 0.3f)
        {
            Handles.DrawLine(targetTransform.position,
                targetTransform.position + (targetTransform.up * distance).RotateHowVector2(angle_offset));
        }
        Handles.color = temp;
#endif
    }

    #endregion DrawZone

    #region DrawCube

    public static void DrawCube(Transform transform, AttackZone attackZone, float offsetRotation)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, -(attackZone.angleOffset - offsetRotation));
        Color temp = Gizmos.color;
        Gizmos.color = attackZone.color;

        Vector3 size = attackZone.cubeSize;
        if (size.x < 0) size.x = 0;
        if (size.y < 0) size.y = 0;
        if (size.z < 0) size.z = 0;
        attackZone.cubeSize = size;

        Gizmos.DrawWireMesh(MeshExtension.CreateCube(), transform.position + (transform.right * attackZone.distance)
            .RotateHowVector2(-rotation.eulerAngles.z), rotation, attackZone.cubeSize);
        Gizmos.color = temp;
    }

    #endregion DrawCube    


    #region DrawCircle

    /// <summary>
    /// рисует 3Д сферу в заданной позиции с заданным радиусом.
    /// цвет задается через Gizmos.color = [чтото]; в OnDrawGizmos/OnDrawGizmosSelected
    /// </summary>
    public static void DrawCircle(Transform position, float radius)
    {
        Gizmos.DrawWireSphere(position.position, radius);
    }

    /// <summary>
    /// рисует 2Д круг в заданной позиции с заданным радиусом
    /// </summary>
    public static void DrawWireArc(Transform position, float radius)
    {
        Color temp = Handles.color;
        Handles.color = Gizmos.color;
        Handles.DrawWireArc(position.position, Vector3.forward, Vector3.up, 360, radius);
        Handles.color = temp;
    }


    #endregion


}