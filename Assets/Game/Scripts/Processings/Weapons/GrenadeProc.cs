using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using System;
using UnityEditor;

public class GrenadeProc : ProcessingBase, ICustomUpdate
{
    Group GrenadeGroup = Group.Create(new ComponentsList<GrenadeCmp, AttackCmp>());

    public void CustomUpdate()
    {
        foreach (int grenade in GrenadeGroup)
        {
            GrenadeCmp grenadeCmp = Storage.GetComponent<GrenadeCmp>(grenade);
            AttackCmp attackCmp = Storage.GetComponent<AttackCmp>(grenade);

            if (OutLiveTime(grenadeCmp))
            {
                AddBoomForce(grenadeCmp, attackCmp);
            }
        }
    }

    bool OutLiveTime(GrenadeCmp grenadeCmp)
    {
        bool outLiveTime = false;

        if (grenadeCmp.current_live_time >= grenadeCmp.total_live_time)
            outLiveTime = true;

        grenadeCmp.current_live_time += Time.deltaTime;

        return outLiveTime;
    }

    void AddBoomForce(GrenadeCmp grenadeCmp, AttackCmp attackCmp)
    {
        Vector3 point1 = grenadeCmp.transform.position + new Vector3(0, 0, 5);
        Vector3 point2 = grenadeCmp.transform.position + new Vector3(0, 0, -5);



        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, grenadeCmp.blast_radius, new Vector3(0, 0, -1), 500, grenadeCmp.attackMask);
        //Debug.Log("Add boom force to " + hits.Length);
        SignalManager<PreparateDamageSignal>.SendSignal(new PreparateDamageSignal(attackCmp, hits));

        /*for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].collider.isTrigger && hits[i].collider.gameObject.TryGetComponent(out EntityBase entity))
            {
                Vector2 forceVector = CalculateBlastForce(grenadeCmp, entity.gameObject);

                Storage.GetComponent<PhysicsCmp>(entity.entity).Rigidbody.AddForce(forceVector);
            }
        }*/

        FinalizeGrenade(grenadeCmp);
    }

    Vector2 CalculateBlastForce(GrenadeCmp grenadeCmp, GameObject TargetObj)
    {
        // расчет силы взрыва по формуле (1 / r) * f,
        // где r - радиус до центра взрыва, f - сила взрыва гранаты
        Vector2 forceDirection = TargetObj.transform.position - grenadeCmp.transform.position;


        // расчет силы взрыва по формуле (R * 1.1 - r) * f,
        // где R - радиус взрыва
        float force_power = (grenadeCmp.blast_radius * 1.1f - forceDirection.magnitude) * grenadeCmp.blast_power;

        return (forceDirection).normalized * force_power;
    }

    void FinalizeGrenade(GrenadeCmp grenadeCmp)
    {
        grenadeCmp.current_live_time = 0;
    }
}
