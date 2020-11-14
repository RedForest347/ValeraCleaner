using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;

public class ExplosionAttackCmp : ComponentBase
{
    public MeleeAttackInfo CurrentAttack { get => attackList[currentAttackIndex]; }

    public MeleeAttackInfo[] attackList;


    public Action<ExplosionAttackCmp> OnAttack;
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
        OnAttack(this);
    }

    private void OnDrawGizmos()
    {
        MoverCmp moverCmp = GetComponent<MoverCmp>();

        if (attackList != null)
        {
            if (moverCmp == null) return;

            for (int i = 0; i < attackList.Length; i++)
            {
                if (attackList[i].attackZone.showZone)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, -(attackList[i].attackZone.angleOffset - moverCmp.rotation));
                    Gizmos.color = attackList[i].attackZone.color;

                    Vector3 pos = attackList[i].attackZone.cubeSize;
                    if (pos.x < 0) pos.x = 0;
                    if (pos.y < 0) pos.y = 0;
                    if (pos.z < 0) pos.z = 0;
                    attackList[i].attackZone.cubeSize = pos;

                    Gizmos.DrawWireMesh(MeshExtension.CreateCube(), transform.position + (transform.right * attackList[i].attackZone.distance)
                        .RotateHowVector2(-rotation.eulerAngles.z), rotation, attackList[i].attackZone.cubeSize);
                }
            }
        }
    }
}

[System.Serializable]
public struct ExplosionAttackInfo
{
    public string attackName;
    public float damage;
    public float pushForce;

    public float attackDelay;
    public float timeBetweenAttacks;
    public EffectBase[] effects;
    public AttackZone attackZone;

    [HideInInspector]
    public float timeAfterLastAttack;
}
