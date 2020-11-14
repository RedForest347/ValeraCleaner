using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class TakeDamageProc : ProcessingBase, ICustomStart, ICustomUpdate, IReceive<DamageSignal>, ICustomDisable
{
    Group DamageGiverGroup = Group.Create(new ComponentsList<MeleeAttackCmp>());
    //Group PhisicsGroup

    public void OnStart()
    {
        SignalManager<DamageSignal>.AddReceiver(this);
        //SignalManager<PreparateDamageSignal>.AddReceiver(this);
    }

    public void CustomUpdate() { }


    public void SignalHandler(DamageSignal arg)
    {
        EntityBase target = arg.attackTarget;
        MeleeAttackCmp attackCmp = arg.attaker.GetCmp<MeleeAttackCmp>();
        AttackInfo attack = arg.attackInfo;


        if (target.TryGetCmp(out HealthCmp healthCmp))
        {
            healthCmp.health -= attack.damage;
        }

        if (target.TryGetCmp(out MoverCmp moverCmp) && target.TryGetCmp(out PhysicsCmp physicsCmp))
        {
            physicsCmp.Rigidbody.AddForce(((Vector2)(target.transform.position - attackCmp.transform.position)) * attack.pushForce);
        }






        Debug.Log("объекту " + arg.attackTarget.name + " нанесен урон " + attack.damage + " от "
            + arg.attaker.gameObject.name + " (" + arg.attaker.entity + ")");
    }

    /*public void SignalHandler(PreparateDamageSignal arg)
    {
        RaycastHit[] castInfos = CreateCorrectRaycastHits(arg.raycastHits);
        int sender = arg.attackSender.entity;
        MeleeAttackInfo attack = arg.attackSender.attackList[arg.attackSender.currentAttackIndex];
        MeleeAttackCmp attackCmp = arg.attackSender;

        for (int i = 0; i < castInfos.Length; i++)
        {
            if (castInfos[i].collider.attachedRigidbody.TryGetComponent(out EntityBase target))
            {
                if (target.entity != sender)
                {
                    SignalManager<DamageSignal>.SendSignal(new DamageSignal(attack, target, attackCmp.entityBase));
                }
            }
        }
    }*/

    public void OnCustomDisable()
    {
        SignalManager<DamageSignal>.RemoveReceiver(this);
        //SignalManager<PreparateDamageSignal>.RemoveReceiver(this);
    }


    static RaycastHit[] CreateCorrectRaycastHits(RaycastHit[] array)
    {
        Dictionary<Rigidbody, RaycastHit> Some = new Dictionary<Rigidbody, RaycastHit>();
        Rigidbody rigidbody;

        for (int i = 0; i < array.Length; i++)
        {
            rigidbody = array[i].collider.attachedRigidbody;
            if (rigidbody != null)
                if (!Some.ContainsKey(rigidbody))
                    Some.Add(rigidbody, array[i]);
        }

        if (Some.Keys.Count == array.Length)
            return array;

        array = new RaycastHit[Some.Keys.Count];

        int index = 0;
        foreach (RaycastHit hit in Some.Values)
            array[index++] = hit;

        return array;
    }
}
