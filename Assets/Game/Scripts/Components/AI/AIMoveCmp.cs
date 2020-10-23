using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using Pathfinding;


[RequireComponent(typeof(AILerp), typeof(Rigidbody2D))]
[Component("AI/AIMove")]
public class AIMoveCmp : ComponentBase, ICustomAwake
{
    
    public Transform target;
    [HideInInspector]
    public AILerp aILerp;
    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public int path_length;
    public float acceleration = 100;
    public float max_speed = 2f;
    public float nearby_distance = 1f;

    [HideInInspector]
    public Vector2 current_move_point;
    [HideInInspector]
    public bool finished;

    //Debug
    [HideInInspector]
    public float distance_to_target;


    public void OnAwake()
    {
        aILerp = GetComponent<AILerp>();
        rb = GetComponent<Rigidbody2D>();
        SetDefoultParans();
    }

    void SetDefoultParans()
    {
        current_move_point.x = float.PositiveInfinity;
        finished = false;
        path_length = -1;
    }
}
