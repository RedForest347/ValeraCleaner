using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;

public class PalyerAttackProc : ProcessingBase, ICustomUpdate
{
    Group AttackGroup = Group.Create(new ComponentsList<AttackCmp, MoverCmp>());


    public void CustomUpdate()
    {



        foreach (int entity in AttackGroup)
        {
            AttackCmp attackCmp = Storage.GetComponent<AttackCmp>(entity);
            RaycastHit[] castInfos;

            if (Input.GetKeyDown(KeyCode.V))
            {
                Attack attack = attackCmp.attackList[0];
                Vector3 pos = attackCmp.transform.position + (Vector3)((Vector2)attackCmp.transform.right * attack.attackZone.distance)
                    .Rotate(attack.attackZone.angleOffset);
                Quaternion rotation = Quaternion.Euler(0, 0, -attack.attackZone.angleOffset);

                castInfos = Physics.BoxCastAll(pos, attack.attackZone.cubeSize / 2, Vector3.forward,
                    rotation, 5, attack.attackZone.layerMask);

                for (int i = 0; i < castInfos.Length; i++)
                {
                    if (castInfos[i].collider.attachedRigidbody.TryGetComponent(out EntityBase target))
                    {
                        SignalManager<DamageSignal>.SendSignal(new DamageSignal(attack, target, attackCmp.entityBase));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {

            }

            if (Input.GetKeyDown(KeyCode.F))
            {

            }

            if (Input.GetKeyDown(KeyCode.G))
            {

            }
        }
    }

    //public
}
