using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public static class Vector2Expansion
{
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        Vector2 rotated_point;
        angle = -Mathf.Deg2Rad * angle;
        rotated_point.x = (float)(vector.x * Math.Cos(angle) - vector.y * Math.Sin(angle));
        rotated_point.y = (float)(vector.x * Math.Sin(angle) + vector.y * Math.Cos(angle));
        return rotated_point;
    }
}
