using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;

public class ExplosionAttackCmp : ComponentBase
{
    public ExplosionAttackInfo CurrentAttack { get => attackList[currentAttackIndex]; }

    public ExplosionAttackInfo[] attackList;


    public Action<ExplosionAttackCmp> OnExplosionAttack;
    [HideInInspector]
    public int currentAttackIndex;
    [HideInInspector]
    public Animator animator;

    public void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public void ExplosionAttack()
    {
        OnExplosionAttack?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        if (attackList != null)
        {
            for (int i = 0; i < attackList.Length; i++)
            {
                if (attackList[i].attackZone.showZone)
                {
                    Gizmos.color = attackList[i].attackZone.color;
                    GizmosExtension.DrawWireArc(transform, attackList[i].radius);
                }
            }
        }
    }
}
