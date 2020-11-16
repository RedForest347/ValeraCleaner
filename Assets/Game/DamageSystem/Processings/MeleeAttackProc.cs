using UnityEngine;
using RangerV;

public class MeleeAttackProc : ProcessingBase, ICustomAwake, ICustomStart, ICustomUpdate, ICustomDisable
{
    Group MeleeAttackGroup = Group.Create(new ComponentsList<MeleeAttackCmp>());

    public void OnAwake()
    {

    }

    public void OnStart()
    {
        MeleeAttackGroup.InitEvents(OnAdd, OnRemove);
    }

    void OnAdd(int ent)
    {
        Storage.GetComponent<MeleeAttackCmp>(ent).OnMeleeAttack += AttackHandler;
    }

    void OnRemove(int ent)
    {
        Storage.GetComponent<MeleeAttackCmp>(ent).OnMeleeAttack -= AttackHandler;
    }

    void AttackHandler(MeleeAttackCmp attackCmp)
    {
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(attackCmp.entity);
        MeleeAttackInfo attack = attackCmp.CurrentAttack;

        RaycastHit[] castInfos = RaycastExtension.BoxRaycast(attackCmp.transform, attack.attackZone, moverCmp.rotation, attack.layerMask);
        EntityBase[] entityBases = castInfos.CreateEntityBaseArray(attackCmp.entity);

        for (int i = 0; i < entityBases.Length; i++)
        {
            AttackInfo attackInfo = ConvertToAttackInfo(attack);

            SignalManager<DamageSignal>.SendSignal(new DamageSignal(attackInfo, entityBases[i], attackCmp.entityBase));
        }
    }

    public AttackInfo ConvertToAttackInfo(MeleeAttackInfo meleeAttackInfo)
    {
        return new AttackInfo
        {
            attackName = meleeAttackInfo.attackName,
            damage = meleeAttackInfo.damage,
            pushForce = meleeAttackInfo.pushForce
        };
    }


    public void CustomUpdate()
    {

    }


    public void OnCustomDisable()
    {
        MeleeAttackGroup.DeinitEvents(OnAdd, OnRemove);
    }
}
