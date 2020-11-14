using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    /// <summary>
    /// поворачивает vector относительно плоскости xy (как будто это Vector2)
    /// </summary>
    public static Vector3 RotateHowVector2(this Vector3 vector, float angle)
    {
        angle = -Mathf.Deg2Rad * angle;
        float x = (float)(vector.x * Math.Cos(angle) - vector.y * Math.Sin(angle));
        float y = (float)(vector.x * Math.Sin(angle) + vector.y * Math.Cos(angle));

        Vector3 rotated_point = new Vector3(x, y, vector.z);
        return rotated_point;
    }
}
