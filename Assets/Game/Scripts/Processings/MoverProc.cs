using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverProc : ProcessingBase, ICustomFixedUpdate
{
    Group MoveGroup = Group.Create(new ComponentsList<MoverCmp, PhysicsCmp>());

    public void CustomFixedUpdate()
    {
        foreach (int entity in MoveGroup)
        {
            Move(entity);
        }
    }

    void Move(int entity)
    {
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(entity);

        Rigidbody rb = Storage.GetComponent<PhysicsCmp>(entity).Rigidbody;
        rb.AddForce(moverCmp.Direction * moverCmp.Acceleration);
        moverCmp.ResetDirection();

        if (rb.velocity.magnitude > moverCmp.MaxSpeed)
            rb.velocity = rb.velocity.normalized * moverCmp.MaxSpeed;
    }

}
