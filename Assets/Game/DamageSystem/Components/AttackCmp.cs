using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class AttackCmp : ComponentBase
{
    public float damage;
    public ElementalDamage elementalDamage;

    public Vector3 CubeCenter;
    public Vector3 CubeSize;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(CubeCenter, CubeSize);
    }
}


[System.Serializable]
public struct ElementalDamage
{
    public float lightningDamage;
    public float toxicDamage;
}
