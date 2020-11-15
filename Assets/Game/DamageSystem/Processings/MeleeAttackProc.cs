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
        Storage.GetComponent<MeleeAttackCmp>(ent).OnAttack += AttackHandler;
    }

    void OnRemove(int ent)
    {
        Storage.GetComponent<MeleeAttackCmp>(ent).OnAttack -= AttackHandler;
    }

    void AttackHandler(MeleeAttackCmp attackCmp)
    {
        //Debug.Log("AttackHandler");
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(attackCmp.entity);
        MeleeAttackInfo attack = attackCmp.CurrentAttack;

        RaycastHit[] castInfos = RaycastExtension.BoxRaycast(attackCmp.transform, attack.attackZone, moverCmp.rotation);
        //Debug.Log("castInfos.Length = " + castInfos.Length);
        EntityBase[] entityBases = castInfos.CreateEntityBaseArray(attackCmp.entity);

        //Debug.Log("entityBases.Length = " + entityBases.Length);

        for (int i = 0; i < entityBases.Length; i++)
        {
            
            AttackInfo attackInfo = ConvertToAttackInfo(attack);

            SignalManager<DamageSignal>.SendSignal(new DamageSignal(attackInfo, entityBases[i], attackCmp.entityBase));
        }

        //SignalManager<PreparateDamageSignal>.SendSignal(new PreparateDamageSignal(attackCmp, castInfos));
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
