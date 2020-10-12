using UnityEngine;
using RangerV;
using System;

public class ColisionProc : ProcessingBase, ICustomStart
{
    Group collision_group = Group.Create(new ComponentsList<Physics2DCmp, Collision2DCmp>());

    public void OnStart()
    {
        collision_group.OnAddEntity += ADDDAction;
    }

    void ADDDAction(int entity)
    {
        Collision2DCmp collision = Storage.GetComponent<Collision2DCmp>(entity);
        collision.AddOnTriggerAction(CollisionHandler, Collision2DActionType.Enter);
        collision.AddOnCollision2DAction(CollisionHandler, Collision2DActionType.Enter);
    }

    void CollisionHandler(int entity)
    {
        Collision2DCmp collision = Storage.GetComponent<Collision2DCmp>(entity);
        bool DoC = collision?.DestroyOnCollision ?? false;
        if (DoC)
        {
            GameObject.Destroy(collision.gameObject);
        }
    }
    public void CollisionHandler(Collider2D collider, int entity)
    {
        CollisionHandler(entity);
    }
    public void CollisionHandler(Collision2D collider, int entity)
    {
        CollisionHandler(entity);
    }



}
