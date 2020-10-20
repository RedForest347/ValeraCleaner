using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class PlayerCmp : ComponentBase, ICustomAwake
{
    public bool should_show;

    public void OnAwake()
    {
        should_show = false;
    }

    private void OnDrawGizmos()
    {
        if (should_show)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 0.05f);
        }
    }
}
