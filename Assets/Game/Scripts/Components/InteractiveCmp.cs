using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using UnityEditor;
using System.Linq;

public class InteractiveCmp : ComponentBase
{
    public float active_distance;
    [Range(0, 360)]
    public float active_angle;
    [Range(-270, 270)]
    public float angle_offset;

    public KeyCode select_key;
    public Action<int> OnSelected;
    public MethodHolder methodHolder;

    public void Select()
    {
        OnSelected?.Invoke(entity);
        methodHolder.StartMethod();
    }


    private void OnDrawGizmosSelected()
    {
        HandlesExpansion.DrawZone(transform, active_angle, angle_offset, active_distance, new Color(0.1f, 0.93f, 0.13f, 0.16f));
    }
}
