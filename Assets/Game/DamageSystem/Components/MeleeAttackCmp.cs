using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;
using UnityEngine.Events;
using System;

public class MeleeAttackCmp : ComponentBase, ICustomAwake
{
    public MeleeAttackInfo CurrentAttack { get => meleeAttackInfos[currentAttackIndex]; }

    public MeleeAttackInfo[] meleeAttackInfos;


    public Action<MeleeAttackCmp> OnMeleeAttack;
    [HideInInspector]
    public int currentAttackIndex;
    [HideInInspector]
    public Animator animator;

    public void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public void MeleeAttack()
    {
        OnMeleeAttack?.Invoke(this);
    }

    public void StartEffect(int effectIndex)
    {
        CurrentAttack.effects[effectIndex].StartPaty();
    }

    private void OnDrawGizmos()
    {
        if (meleeAttackInfos != null)
        {
            for (int i = 0; i < meleeAttackInfos.Length; i++)
            {
                if (meleeAttackInfos[i].attackZone.showZone)
                {
                    float rotationOffset = entityBase.GetCmp<MoverCmp>()?.rotation ?? 0;
                    GizmosExtension.DrawCube(transform, meleeAttackInfos[i].attackZone, rotationOffset);
                }
            }
        }
    }
}
