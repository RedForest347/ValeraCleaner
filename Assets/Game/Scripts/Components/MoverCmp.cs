using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class MoverCmp : ComponentBase
{
    public float Acceleration;
    public float MaxSpeed;


    [SerializeField, Tooltip("0 градусов - вправа, увеличение против часовой стрелки")]
    float rotation;
    public float NormalisedRotation // возвращает значение поварота от 0 - 360
    {
        get => rotation < 0 ? rotation + 360 : rotation;
    }

    Vector2 direction;
    public Vector2 Direction { get => direction; }

    public void AddDirection(Vector2 dir)
    {
        direction += dir.normalized;
        direction = direction.normalized;
    }
    
    
    public void ResetDirection()
    {
        direction = new Vector2();
    }


    public void SetRotation(float new_rotation)
    {
        rotation = new_rotation;
    }

}
