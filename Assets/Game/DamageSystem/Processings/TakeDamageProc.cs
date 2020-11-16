using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class TakeDamageProc : ProcessingBase, ICustomStart, ICustomUpdate, IReceive<DamageSignal>, ICustomDisable
{


    public void OnStart()
    {
        SignalManager<DamageSignal>.AddReceiver(this);
        //SignalManager<PreparateDamageSignal>.AddReceiver(this);
    }

    public void CustomUpdate() { }


    public void SignalHandler(DamageSignal arg)
    {
        EntityBase target = arg.attackTarget;
        EntityBase attacker = arg.attaker;
        AttackInfo attack = arg.attackInfo;


        if (target.TryGetCmp(out HealthCmp healthCmp))
        {
            healthCmp.health -= attack.damage;
        }

        if (target.TryGetCmp(out MoverCmp moverCmp) && target.TryGetCmp(out PhysicsCmp physicsCmp))
        {
            physicsCmp.Rigidbody.AddForce(((Vector2)(target.transform.position - attacker.transform.position)).normalized * attack.pushForce);
        }






        //Debug.Log("объекту " + arg.attackTarget.name + " нанесен урон " + attack.damage + " от "
        //    + arg.attaker.gameObject.name + " (" + arg.attaker.entity + ")");
    }

    public void OnCustomDisable()
    {
        SignalManager<DamageSignal>.RemoveReceiver(this);
    }
}
