using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProc : ProcessingBase, ICustomUpdate
{
    Group MoveGroup = Group.Create(new ComponentsList<MoveCmp>());

    public void CustomUpdate()
    {
        foreach (int entity in MoveGroup)
        {
            Vector2 velocity = new Vector2();

            float speed = Storage.GetComponent<MoveCmp>(entity).speed;


            if (Input.GetKey(KeyCode.W))
            {
                velocity.y = speed;
            }

            else if (Input.GetKey(KeyCode.S))
            {
                velocity.y = -speed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -speed;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                velocity.x = speed;
            }


            //EntityBase.GetEntity(entity).transform.Translate(new Vector3(velocity.x, velocity.y, 0));
            EntityBase.GetEntity(entity).GetComponent<Rigidbody2D>().velocity = velocity;
            EntityBase.GetEntity(entity).GetComponent<Rigidbody2D>().angularVelocity = 4;

            //EntityBase.GetEntity(entity).GetComponent<Rigidbody2D>().
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
    }
}
