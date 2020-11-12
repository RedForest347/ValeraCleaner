using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;
using UnityEngine.Events;
using System;

public class AttackCmp : ComponentBase, ICustomAwake
{
    public Attack CurrentAttack { get => attackList[currentAttackIndex]; }

    public Attack[] attackList;


    public Action<AttackCmp> OnAttack;
    [HideInInspector]
    public int currentAttackIndex;
    [HideInInspector]
    public Animator animator;

    public void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public void Kick()
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

                    Gizmos.DrawWireMesh(MeshExpansion.CreateCube(), transform.position + (transform.right * attackList[i].attackZone.distance)
                        .RotateHowVector2(-rotation.eulerAngles.z), rotation, attackList[i].attackZone.cubeSize);
                }
            }
        }
    }
}

[System.Serializable]
public struct Attack
{
    public string attackName;

    public float damage;

    //public UnityEvent unityEvent;
    public float timeBetweenAttacks;
    [HideInInspector]
    public float timeAfterLastAttack;

    public float pushForce;

    public EffectBase[] effects;
    public AttackZone attackZone;
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
