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

    void Move(int entity) // при тольчке извне скорость объекта почти моментально становиться равна максимально допустимой, хотя толчек должен действовать полноценно
    {
        MoverCmp moverCmp = Storage.GetComponent<MoverCmp>(entity);

        Rigidbody rb = Storage.GetComponent<PhysicsCmp>(entity).Rigidbody;

        /*Vector2 newVelicity = (Vector2)rb.velocity + moverCmp.Direction * (moverCmp.Acceleration / rb.mass * Time.fixedDeltaTime);
        float new_magnitude = newVelicity.magnitude;
        //Debug.Log("new_magnitude = " + new_magnitude);

        if (new_magnitude > moverCmp.MaxSpeed)
        {
            if (new_magnitude >= rb.velocity.magnitude)
            {
                return;
            }
        }*/

        rb.AddForce(moverCmp.Direction * moverCmp.Acceleration);
        moverCmp.ResetDirection();

        /*if (rb.velocity.magnitude > moverCmp.MaxSpeed)
            rb.velocity = rb.velocity.normalized * moverCmp.MaxSpeed;*/
    }

}
