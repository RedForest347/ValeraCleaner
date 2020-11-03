using UnityEngine;
using RangerV;
using System;

public class ColisionDamageProc : ProcessingBase, ICustomUpdate, ICustomStart
{
    Group collision_group = Group.Create(new ComponentsList<CollisionDamageCmp, PhysicsCmp, CollisionCmp>());
    Group target_group = Group.Create(new ComponentsList<HealthCmp>());


    public void OnStart()
    {
        collision_group.OnAddEntity += ADDDAction;
    }

    void ADDDAction(int entity)
    {
        Storage.GetComponent<CollisionCmp>(entity).AddOnTriggerAction(GiveDamage, CollisionActionType.Enter);
    }


    public void CustomUpdate()
    {

    }


    public void GiveDamage(Collider collider, int entity)
    {
        int target_entity = collider.gameObject.GetComponent<Entity>()?.entity ?? -1;

        if (target_group.Contains(target_entity))
        {
            HealthCmp healthComponent = Storage.GetComponent<HealthCmp>(target_entity);
            healthComponent.health -= 5;
            if (healthComponent.health <= 0)
                GameObject.Destroy(collider.gameObject);

            CollisionDamageCmp collisionDamage = Storage.GetComponent<CollisionDamageCmp>(entity);
            bool DoC = collisionDamage?.DestroyOnCollision ?? false;
            Debug.Log("<CollisionDamageCmp>(entity) = " + Storage.GetComponent<CollisionDamageCmp>(entity));
            Debug.Log("Storage.GetComponent<CollisionDamageCmp>(entity)?.DestroyOnCollision = " + Storage.GetComponent<CollisionDamageCmp>(entity)?.DestroyOnCollision);
            if (DoC)
            {
                //Debug.Log("Storage.GetComponent<GameObjectComponent>(entity) = " + Storage.GetComponent<GameObjectComponent>(entity));
                GameObject.Destroy(collisionDamage.gameObject);
            }
        }
    }

   
}
