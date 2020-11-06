using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class TakeDamageProc : ProcessingBase, ICustomStart, ICustomUpdate, IReceive<DamageSignal>, ICustomDisable
{
    Group DamageGiverGroup = Group.Create(new ComponentsList<AttackCmp>());

    public void OnStart()
    {
        SignalManager<DamageSignal>.AddReceiver(this);
    }

    public void CustomUpdate()
    {
        /*foreach (int damageGiver in DamageGiverGroup)
        {
            DamageGiverCmp damageGiverCmp = Storage.GetComponent<DamageGiverCmp>(damageGiver);

            damageGiverCmp.time_after_last_attack += Time.deltaTime;
        }*/
    }

    public void SignalHandler(DamageSignal arg)
    {
        //arg.attackCmp.time_after_last_attack = 0;
        Debug.Log("объекту " + arg.attackTarget.name + " нанесен урон " + arg.attackParameters.damage + " от " 
            + arg.attaker.gameObject.name + " (" + arg.attaker.entity + ")");
        Storage.GetComponent<HealthCmp>(arg.attackTarget.entity).health -= arg.attackParameters.damage;
    }


    public void OnCustomDisable()
    {
        SignalManager<DamageSignal>.RemoveReceiver(this);
    }
}
