using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class TakeDamageProc : ProcessingBase, ICustomStart, ICustomUpdate, IReceive<DamageSignal>, ICustomDisable
{
    Group DamageGiverGroup = Group.Create(new ComponentsList<DamageGiverCmp>());

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
        arg.damageGiver.time_after_last_attack = 0;
        Debug.Log("объекту " + arg.damageTaker.name + " нанесен урон " + arg.damageGiver.damage + " от " + arg.damageGiver.entityBase.name + " (" + arg.damageGiver.entity + ")");
    }


    public void OnCustomDisable()
    {
        SignalManager<DamageSignal>.RemoveReceiver(this);
    }
}
