using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public static class Vector2Extension
{
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        angle = -Mathf.Deg2Rad * angle;
        float x = (float)(vector.x * Math.Cos(angle) - vector.y * Math.Sin(angle));
        float y = (float)(vector.x * Math.Sin(angle) + vector.y * Math.Cos(angle));

        Vector2 rotated_point = new Vector2(x, y);
        return rotated_point;
    }
}
