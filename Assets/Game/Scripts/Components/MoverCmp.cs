using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class MoverCmp : ComponentBase
{
    public float Acceleration;
    public float MaxSpeed;

    //[HideInInspector]
    Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
    }

    public void AddDirection(Vector2 dir)
    {
        direction += dir.normalized;
    }
    
    
    public void ResetDirection()
    {
        direction = new Vector2();
    }

}
