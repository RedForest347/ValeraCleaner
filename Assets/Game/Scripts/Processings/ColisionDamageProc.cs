using UnityEngine;
using RangerV;
using System;

public class ColisionDamageProc : ProcessingBase, ICustomUpdate, ICustomStart
{
    Group collision_group = Group.Create(new ComponentsList<CollisionDamageCmp, Physics2DCmp, Collision2DCmp>());
    Group target_group = Group.Create(new ComponentsList<HealthCmp, GameObjectCmp>());


    public void OnStart()
    {
        collision_group.OnAddEntity += ADDDAction;
    }

    void ADDDAction(int entity)
    {
        Storage.GetComponent<Collision2DCmp>(entity).AddOnTriggerAction(GiveDamage, Collision2DActionType.Enter);
    }


    public void CustomUpdate()
    {

    }


    public void GiveDamage(Collider2D collider, int entity)
    {
        int target_entity = collider.gameObject.GetComponent<Entity>().entity;

        if (target_group.Contains(target_entity))
        {
            HealthCmp healthComponent = Storage.GetComponent<HealthCmp>(target_entity);
            healthComponent.health -= 5;
            if (healthComponent.health <= 0)
                GameObject.Destroy(Storage.GetComponent<GameObjectCmp>(target_entity).GameObject);

            if (Storage.GetComponent<CollisionDamageCmp>(entity).destroy_on_collision)
            {
                //Debug.Log("Storage.GetComponent<GameObjectComponent>(entity) = " + Storage.GetComponent<GameObjectComponent>(entity));
                GameObject.Destroy(Storage.GetComponent<GameObjectCmp>(entity).GameObject);
            }
        }
    }

   
}
