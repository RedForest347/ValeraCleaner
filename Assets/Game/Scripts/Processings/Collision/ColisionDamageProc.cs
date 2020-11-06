using UnityEngine;
using RangerV;
using System;

public class ColisionDamageProc : ProcessingBase, ICustomUpdate, ICustomStart, ICustomDisable
{
    Group collision_group = Group.Create(new ComponentsList<CollisionDamageCmp, PhysicsCmp, CollisionCmp>());
    Group target_group = Group.Create(new ComponentsList<HealthCmp>());


    public void OnStart()
    {
        collision_group.InitEvents(ADDDAction, RemoveAction);
    }

    void ADDDAction(int entity)
    {
        Storage.GetComponent<CollisionCmp>(entity).AddOnTriggerAction(GiveDamage, CollisionActionType.Enter);
    }

    void RemoveAction(int entity)
    {
        Storage.GetComponent<CollisionCmp>(entity).RemoveOnTriggerAction(GiveDamage, CollisionActionType.Enter);
    }


    public void CustomUpdate()
    {

    }


    public void GiveDamage(Collider damage_recipient, int damage_giver)
    {
        int target_entity = damage_recipient.attachedRigidbody?.GetComponent<Entity>()?.entity ?? damage_recipient.GetComponent<Entity>()?.entity ?? - 1;

        if (target_group.Contains(target_entity))
        {
            HealthCmp healthComponent = Storage.GetComponent<HealthCmp>(target_entity);
            healthComponent.health -= 5;
            if (healthComponent.health <= 0)
                GameObject.Destroy(damage_recipient.gameObject);

            CollisionDamageCmp collisionDamage = Storage.GetComponent<CollisionDamageCmp>(damage_giver);
            bool DoC = collisionDamage?.DestroyOnCollision ?? false;

            if (DoC)
            {
                GameObject.Destroy(collisionDamage.gameObject);
            }
        }
    }

    public void OnCustomDisable()
    {
        collision_group.DeinitEvents(ADDDAction, RemoveAction);
    }
}
