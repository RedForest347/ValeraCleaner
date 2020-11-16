using UnityEngine;
using RangerV;
using System;
using System.Collections.Generic;
using System.Collections;

public class ExplosionAttackProc : ProcessingBase, ICustomAwake, ICustomStart, ICustomUpdate, ICustomDisable
{
    Group explosionAttackGroup = Group.Create(new ComponentsList<ExplosionAttackCmp>());

    public void OnAwake()
    {

    }

    public void OnStart()
    {
        explosionAttackGroup.InitEvents(OnAdd, OnRemove);
        CorutineManager.StartCorutine(BoomTemp());
    }


    IEnumerator BoomTemp()
    {
        while (true)
        {
            for (int i = 0; i < 100; i++)
            {
                yield return null;
            }

            foreach (int ent in explosionAttackGroup)
            {
                Storage.GetComponent<ExplosionAttackCmp>(ent).ExplosionAttack();
            }
        }
    }

    void OnAdd(int ent)
    {
        Storage.GetComponent<ExplosionAttackCmp>(ent).OnExplosionAttack += AttackHandler;
    }

    void OnRemove(int ent)
    {
        Storage.GetComponent<ExplosionAttackCmp>(ent).OnExplosionAttack -= AttackHandler;
    }

    void AttackHandler(ExplosionAttackCmp explosionAttackCmp)
    {
        ExplosionAttackInfo explosionAttackInfo = explosionAttackCmp.CurrentAttack;
        RaycastHit[] castInfos = RaycastExtension.CircleRaycast(explosionAttackCmp.transform, explosionAttackInfo.radius, explosionAttackInfo.layerMask);

        EntityBase[] targets = castInfos.CreateEntityBaseArray();

        for (int i = 0; i < targets.Length; i++)
        {
            CalculateBlastForceAndDamage(ref explosionAttackInfo, explosionAttackCmp.transform, targets[i].transform);
            AttackInfo attackInfo = ConvertToAttackInfo(explosionAttackInfo);
            SignalManager<DamageSignal>.SendSignal(new DamageSignal(attackInfo, targets[i], explosionAttackCmp.entityBase));
        }
    }

    AttackInfo ConvertToAttackInfo(ExplosionAttackInfo explosionAttackInfo)
    {
        return new AttackInfo
        {
            attackName = explosionAttackInfo.attackName,
            damage = explosionAttackInfo.calculatedDamage,
            pushForce = explosionAttackInfo.calculatedPushForce
        };
    }


    /// <summary>
    /// считает урон и силу отталкивания на основе дистанции до цели по формуле:
    /// coef = (radius - distance) / distance
    /// value = min + (max - min) * coef
    /// т.е. урон интерпалируется по дистанции до цели
    /// </summary>
    void CalculateBlastForceAndDamage(ref ExplosionAttackInfo explosionAttackInfo, Transform damageGiver, Transform targetObj)
    {
        float distanceToTarget = ((Vector2)(damageGiver.transform.position - targetObj.transform.position)).magnitude;

        if (distanceToTarget > explosionAttackInfo.radius) //?
        {
            explosionAttackInfo.calculatedDamage = 0;
            explosionAttackInfo.calculatedPushForce = 0;
            return;
        }

        float distanceCoef = (explosionAttackInfo.radius - distanceToTarget) / explosionAttackInfo.radius;

        ValueSpread dSpread = explosionAttackInfo.damageSpread;
        ValueSpread pfSpread = explosionAttackInfo.pushForseSpread;

        float calculatedDamage, calculatedPushForce;

        calculatedDamage = dSpread.min + (dSpread.max - dSpread.min) * distanceCoef;
        
        calculatedPushForce = pfSpread.min + (pfSpread.max - pfSpread.min) * distanceCoef;

        explosionAttackInfo.calculatedDamage = calculatedDamage;
        explosionAttackInfo.calculatedPushForce = calculatedPushForce;

    }

    void CalculateBlastForceAndDamageV1(ref ExplosionAttackInfo explosionAttackInfo, Transform damageGiver, Transform targetObj)
    {
        float distanceToTarget = ((Vector2)(damageGiver.transform.position - targetObj.transform.position)).magnitude;

        if (distanceToTarget > explosionAttackInfo.radius) //?
        {
            explosionAttackInfo.calculatedDamage = 0;
            explosionAttackInfo.calculatedPushForce = 0;
            return;
        }

        float distanceCoef = (explosionAttackInfo.radius - distanceToTarget) / explosionAttackInfo.radius;
        //distanceCoef = Math.Max(distanceCoef, 0);

        ValueSpread dSpread = explosionAttackInfo.damageSpread;
        ValueSpread pfSpread = explosionAttackInfo.pushForseSpread;


        float calculatedDamage, calculatedPushForce;

        //Debug.Log("dSpread.min = " + dSpread.min + " dSpread.max = " + dSpread.max);
        //Debug.Log("pfSpread.min = " + pfSpread.min + " pfSpread.max = " + pfSpread.max);

        /*if (dSpread.max > dSpread.min && dSpread.max > 0)*/
        calculatedDamage = dSpread.min + (dSpread.max - dSpread.min) * distanceCoef;
        /*else calculatedDamage = 0;*/

        /*if (pfSpread.max > pfSpread.min && pfSpread.max > 0)*/
        calculatedPushForce = pfSpread.min + (pfSpread.max - pfSpread.min) * distanceCoef;
        /*else calculatedPushForce = 0;*/

        //Debug.Log("calculatedDamage = " + calculatedDamage + " calculatedPushForce = " + calculatedPushForce);

        explosionAttackInfo.calculatedDamage = calculatedDamage;
        explosionAttackInfo.calculatedPushForce = calculatedPushForce;

    }

    public void CustomUpdate()
    {

    }


    public void OnCustomDisable()
    {
        explosionAttackGroup.DeinitEvents(OnAdd, OnRemove);
    }
}
