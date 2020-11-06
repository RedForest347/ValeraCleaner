using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;

public class PalyerAttackProc : ProcessingBase, ICustomUpdate
{
    Group AttackGroup = Group.Create(new ComponentsList<AttackCmp>());

    public void CustomUpdate()
    {
        foreach (int entity in AttackGroup)
        {
            AttackCmp attackCmp = Storage.GetComponent<AttackCmp>(entity);
            RaycastHit[] castInfos = Physics.BoxCastAll(attackCmp.CubeCenter, attackCmp.CubeSize / 2, Vector3.up/*, Quaternion.identity, LayerMask*/);

            for (int i = 0; i < castInfos.Length; i++)
            {
                Debug.Log(castInfos[i].collider.gameObject.name);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                
                //Physics.BoxCastAll();
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
}
