using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;

public class PalyerAttackProc : ProcessingBase, ICustomUpdate
{
    Group AttackGroup = Group.Create(new ComponentsList<MeleeAttackCmp, MoverCmp>());

    public void CustomUpdate()
    {



        foreach (int entity in AttackGroup)
        {
            MeleeAttackCmp meleeAttackCmp = Storage.GetComponent<MeleeAttackCmp>(entity);

            if (Input.GetKeyDown(KeyCode.V))
            {
                meleeAttackCmp.currentAttackIndex = 0;
                meleeAttackCmp.animator.SetTrigger("Kick");
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                meleeAttackCmp.currentAttackIndex = 1;
                meleeAttackCmp.animator.SetTrigger("JumpKick");
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

            }

            if (Input.GetKeyDown(KeyCode.G))
            {

            }
        }
    }
}
