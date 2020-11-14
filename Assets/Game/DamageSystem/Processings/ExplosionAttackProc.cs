using UnityEngine;
using RangerV;

public class ExplosionAttackProc : ProcessingBase, ICustomAwake, ICustomStart, ICustomUpdate, ICustomDisable
{
    Group MeleeAttackGroup = Group.Create(new ComponentsList<ExplosionAttackCmp>());

    public void OnAwake()
    {

    }

    public void OnStart()
    {
        MeleeAttackGroup.InitEvents(OnAdd, OnRemove);
    }

    void OnAdd(int ent)
    {
        Storage.GetComponent<ExplosionAttackCmp>(ent).OnAttack += AttackHandler;
    }

    void OnRemove(int ent)
    {
        Storage.GetComponent<ExplosionAttackCmp>(ent).OnAttack -= AttackHandler;
    }

    void AttackHandler(ExplosionAttackCmp attackCmp)
    {
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(attackCmp.entity);
        MeleeAttackInfo attack = attackCmp.CurrentAttack;
        RaycastHit[] castInfos;

        Quaternion rotation = Quaternion.Euler(0, 0, -attack.attackZone.angleOffset + moverCmp.rotation);
        Vector3 pos = attackCmp.transform.position + (attackCmp.transform.right * attack.attackZone.distance)
            .RotateHowVector2(-rotation.eulerAngles.z);

        castInfos = Physics.BoxCastAll(pos, attack.attackZone.cubeSize / 2, Vector3.forward, rotation, 5, attack.attackZone.layerMask);

        //SignalManager<PreparateDamageSignal>.SendSignal(new PreparateDamageSignal(attackCmp, castInfos));
    }

    public void CustomUpdate()
    {

    }


    public void OnCustomDisable()
    {
        MeleeAttackGroup.DeinitEvents(OnAdd, OnRemove);
    }
}
