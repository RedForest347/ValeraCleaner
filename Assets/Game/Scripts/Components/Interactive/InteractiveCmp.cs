using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using UnityEditor;
using System.Linq;
using UnityEngine.Events;

public class InteractiveCmp : ComponentBase, ICustomAwake
{
    [Min(0)]
    public float active_distance;
    [Range(0, 360)]
    public float active_angle;
    [Range(-270, 270)]
    public float angle_offset;

    public KeyCode select_key;

    [HideInInspector]
    public bool was_select_in_previous_frame;
    [HideInInspector]
    public bool select_in_current_frame;

    public Action<int> OnSelect;
    public UnityEvent SelectUE;

    public Action<int> OnZoneEnter;
    public UnityEvent EnterUE;

    public Action<int> OnZoneStay;
    public UnityEvent StayUE;

    public Action<int> OnZoneExit;
    public UnityEvent ExitUE;

    public void OnAwake()
    {
        was_select_in_previous_frame = false;
    }

    private void OnDrawGizmosSelected()
    {
        HandlesExpansion.DrawZone(transform, active_angle, angle_offset, active_distance, new Color(0.1f, 0.93f, 0.13f, 0.16f));
    }
}
