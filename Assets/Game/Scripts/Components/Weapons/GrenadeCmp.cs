using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;


/// <summary>
/// 
/// </summary>
[Component("Weapon/GrenadeCmp")]
public class GrenadeCmp : ComponentBase, ICustomAwake
{
    public float blast_power; // сила взрыва
    public float blast_radius;

    public float total_live_time; // через сколько секунд граната взрывается
    public float current_live_time; // сколько времени жизни гранаты прошло

    public void OnAwake()
    {
        current_live_time = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, blast_radius);
    }
}
