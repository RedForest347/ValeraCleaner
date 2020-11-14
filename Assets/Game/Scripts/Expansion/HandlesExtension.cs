using System;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions.Must;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class HandlesExtension
{


    #region DrawZone

    /// <summary>
    /// рисует зону окружности
    /// зона рисуется по сторонам от вектора targetTransform.up + angle_offset
    /// </summary>
    /// <param name="targetTransform">трансформ, относительно которого рисуется зона</param>
    public static void DrawZone(this Gizmos gizmos, Transform targetTransform, float angle, float distance, Color color)
    {
        DrawZoneBase(targetTransform, targetTransform.up, angle, 0, distance, color);
    }


    public static void DrawZone(Transform targetTransform, float angle, float distance, Color color)
    {
        DrawZoneBase(targetTransform, targetTransform.up, angle, 0, distance, color);
    }

    public static void DrawZone(this Gizmos gizmos, Transform targetTransform, float angle, float angle_offset, float distance, Color color)
    {
        Vector3 from = Vector2Extension.Rotate(targetTransform.up, angle_offset);
        DrawZoneBase(targetTransform, from, angle, angle_offset, distance, color);
    }

    public static void DrawZone(Transform targetTransform, float angle, float angle_offset, float distance, Color color)
    {
        Vector3 from = Vector2Extension.Rotate(targetTransform.up, angle_offset);
        DrawZoneBase(targetTransform, from, angle, angle_offset, distance, color);
    }

    static void DrawZoneBase(Transform targetTransform, Vector3 from, float angle, float angle_offset, float distance, Color color)
    {
#if UNITY_EDITOR
        angle = angle / 2;
        Handles.color = color;
        Handles.DrawSolidArc(targetTransform.position, Vector3.forward, from, angle, distance);
        Handles.DrawSolidArc(targetTransform.position, Vector3.forward, from, -angle, distance);
        Handles.color = Color.black;
        if (angle < 180 - 0.5f)
        {
            Handles.DrawLine(targetTransform.position,
                targetTransform.position + (targetTransform.up * distance).RotateHowVector2(angle_offset));
        }
#endif
    }

    #endregion DrawZone


}