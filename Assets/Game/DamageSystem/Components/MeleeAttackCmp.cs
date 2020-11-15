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


    public Action<MeleeAttackCmp> OnAttack;
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
        OnAttack?.Invoke(this);
    }

    public void StartEffect(int effectIndex)
    {
        CurrentAttack.effects[effectIndex].StartPaty();
    }

    private void OnDrawGizmos()
    {
        MoverCmp moverCmp = GetComponent<MoverCmp>();

        if (meleeAttackInfos != null && moverCmp != null)
        {
            for (int i = 0; i < meleeAttackInfos.Length; i++)
            {
                if (meleeAttackInfos[i].attackZone.showZone)
                {
                    GizmosExtension.DrawCube(transform, meleeAttackInfos[i].attackZone, moverCmp.rotation);
                }
            }
        }
    }
}

[System.Serializable]
public struct MeleeAttackInfo
{
    public string attackName;
    public float damage;
    public float pushForce;

    public float attackDelay;
    public float timeBetweenAttacks;
    //public EffectBase[] effects;
    public PatyManager[] effects;
    public AttackZone attackZone;

    [HideInInspector]
    public float timeAfterLastAttack;
}


[System.Serializable]
public struct AttackInfo
{
    public string attackName;
    public float damage;
    public float pushForce;
}

[System.Serializable]
public class AttackZone
{
    public bool showZone;
    public Color color = Color.blue;

    public LayerMask layerMask;
    public float distance;
    public float angleOffset;
    public Vector3 cubeSize;

}
