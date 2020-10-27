using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using Pathfinding;
using System;

[RequireComponent(typeof(AILerp))]
[Component("AI/AIMove")]
public class AIMoveCmp : ComponentBase, ICustomAwake
{
    //public Transform target;
    public Vector3 target;// { get; private set; }

    [Min(0)]
    public float acceleration = 100;
    [Min(0)]
    public float max_speed = 2f;
    [Min(0)]
    public float nearby_distance; // дистанция, при которой считается что путь пройден. используется для AIMoveProc. 
                                  // проверку на то, рядом ли цель в превую очередь следует осуществлять в Proc, State и из иных мест
                                  // данная поле показывает то, что путь исчерпан, идти дальше некуда, и пора сообщить об этом через вызов OnReached
    public AIMoveMode moveMode;

    public Action<int> OnReached; // вошел в зону цели
    public Action<int> OnStop;

    [HideInInspector]
    public AILerp aILerp;

    [HideInInspector]
    //public Rigidbody2D rb;
    public Rigidbody rb;

    [HideInInspector]
    public Vector2 current_move_point;

    [HideInInspector]
    public int path_length;

    //[HideInInspector]
    bool _finished;
    //public bool can_move;

    public bool finished
    {
        get => _finished;
        set
        {
            if (value == true)
            {
                OnReached(entity);
            }
            _finished = value;
        }
    }

    //Debug
    [HideInInspector]
    public float distance_to_target;


    public void OnAwake()
    {
        moveMode = AIMoveMode.Sleep;
        aILerp = GetComponent<AILerp>();
        rb = GetComponent<Rigidbody>();
        SetDefoultParans();
    }

    public void SetTarget(Transform target)
    {
        SetTarget(target.position);
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        aILerp.ResetPathCustom();
    }



    void SetDefoultParans()
    {
        current_move_point.x = float.PositiveInfinity;
        finished = false;
        path_length = -1;
    }
}

public enum AIMoveMode
{
    GoToTarget,
    Stopping,
    Sleep
}
