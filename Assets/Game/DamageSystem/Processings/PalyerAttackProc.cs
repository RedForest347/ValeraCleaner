using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;

public class PalyerAttackProc : ProcessingBase, ICustomUpdate, ICustomStart, ICustomDisable
{
    Group AttackGroup = Group.Create(new ComponentsList<AttackCmp, MoverCmp>());


    public void OnStart()
    {
        AttackGroup.InitEvents(OnAdd, OnRemove);
    }

    void OnAdd(int entity)
    {
        Storage.GetComponent<AttackCmp>(entity).OnAttack += AttackHandler;
    }

    void OnRemove(int entity)
    {
        Storage.GetComponent<AttackCmp>(entity).OnAttack -= AttackHandler;
    }



    public void CustomUpdate()
    {



        foreach (int entity in AttackGroup)
        {
            AttackCmp attackCmp = Storage.GetComponent<AttackCmp>(entity);

            if (Input.GetKeyDown(KeyCode.V))
            {
                attackCmp.currentAttackType = 0;
                attackCmp.animator.SetTrigger("Kick");
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                attackCmp.currentAttackType = 1;
                attackCmp.animator.SetTrigger("JumpKick");
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

            }

            if (Input.GetKeyDown(KeyCode.G))
            {

            }
        }



    }

    void AttackHandler(int sender, Attack attack)
    {
        AttackCmp attackCmp = Storage.GetComponent<AttackCmp>(sender);
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(sender);
        RaycastHit[] castInfos;

        Quaternion rotation = Quaternion.Euler(0, 0, -attack.attackZone.angleOffset + moverCmp.rotation);
        Vector3 pos = attackCmp.transform.position + (attackCmp.transform.right * attack.attackZone.distance)
            .RotateHowVector2(-rotation.eulerAngles.z);

        castInfos = Physics.BoxCastAll(pos, attack.attackZone.cubeSize / 2, Vector3.forward, rotation, 5, attack.attackZone.layerMask);

        castInfos = SupportFunction.CreateCorrectRaycastHits(castInfos);


        for (int i = 0; i < castInfos.Length; i++)
        {
            if (castInfos[i].collider.attachedRigidbody.TryGetComponent(out EntityBase target))
            {
                if (target.entity != sender)
                {
                    target.GetComponent<Rigidbody>().AddForce(((Vector2)target.transform.position - (Vector2)attackCmp.transform.position) * attack.pushForce);
                    //attackCmp.GetComponent<Rigidbody>().AddForce(-((Vector2)target.transform.position - (Vector2)attackCmp.transform.position) * attack.pushForce);

                    SignalManager<DamageSignal>.SendSignal(new DamageSignal(attack, target, attackCmp.entityBase));
                }
            }
        }
    }

    public void OnCustomDisable()
    {
        AttackGroup.DeinitEvents(OnAdd, OnRemove);
    }

}
