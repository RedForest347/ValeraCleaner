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

        if (meleeAttackInfos != null)
        {
            if (moverCmp == null) return;

            for (int i = 0; i < meleeAttackInfos.Length; i++)
            {
                if (meleeAttackInfos[i].attackZone.showZone)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, -(meleeAttackInfos[i].attackZone.angleOffset - moverCmp.rotation));
                    Gizmos.color = meleeAttackInfos[i].attackZone.color;

                    Vector3 pos = meleeAttackInfos[i].attackZone.cubeSize;
                    if (pos.x < 0) pos.x = 0;
                    if (pos.y < 0) pos.y = 0;
                    if (pos.z < 0) pos.z = 0;
                    meleeAttackInfos[i].attackZone.cubeSize = pos;

                    Gizmos.DrawWireMesh(MeshExtension.CreateCube(), transform.position + (transform.right * meleeAttackInfos[i].attackZone.distance)
                        .RotateHowVector2(-rotation.eulerAngles.z), rotation, meleeAttackInfos[i].attackZone.cubeSize);
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
