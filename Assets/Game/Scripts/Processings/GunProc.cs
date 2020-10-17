using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class GunProc : ProcessingBase, ICustomFixedUpdate
{
    Group GunGroup = Group.Create(new ComponentsList<GunCmp>());

    public void CustomFixedUpdate()
    {
        foreach (int entity in GunGroup)
        {
            Shooting(entity);
        }
    }

    void Shooting(int entity)
    {
        GunCmp gunCmp = Storage.GetComponent<GunCmp>(entity);

        if (gunCmp.shooting && gunCmp.timer >= gunCmp.rateOfFire)
        {
            int entity_bullet = GameObject.Instantiate(gunCmp.Bullet, gunCmp.transform.position, gunCmp.transform.rotation).GetComponent<Entity>().entity;
            Storage.GetComponent<Collision2DCmp>(entity_bullet).IgnoreColliders.AddRange(Storage.GetComponent<GunCmp>(entity).IgnoreColliders);
            gunCmp.timer = 0;
        }

        gunCmp.timer++;
        if(gunCmp.timer > 10000000)
            gunCmp.timer = gunCmp.rateOfFire;
    }

    
}
