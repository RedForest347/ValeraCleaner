using RangerV;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerProc2 : ProcessingBase, ICustomUpdate, ICustomFixedUpdate
{
    Group MoveGroup = Group.Create(new ComponentsList<PlayerControllerCmp, Physics2DCmp>());
    //Group ShootGroup = Group.Create(new ComponentsList<GunControllerCmp>());

    public void CustomUpdate()
    {

    }

    public void CustomFixedUpdate()
    {
        foreach (int entity in MoveGroup)
        {
            Move(entity);
            Shooting(entity);
        }
        //foreach (int entity in ShootGroup)
        //{
        //    Shooting(entity);
        //}
    }

    void Move(int entity)
    {
        Vector2 velocity = new Vector2();

        float speed = Storage.GetComponent<PlayerControllerCmp>(entity).speed;
        GameObject Hand = Storage.GetComponent<PlayerControllerCmp>(entity).Hand;

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


        //EntityBase.GetEntity(entity).GetComponent<Rigidbody2D>().velocity = velocity;
        Storage.GetComponent<Physics2DCmp>(entity).Rigidbody.velocity = velocity;

        float angle = FindRotateAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition), Hand.transform.position);
        //EntityBase.GetEntity(entity).GetComponent<Rigidbody2D>().SetRotation(angle);
        Hand.transform.eulerAngles = new Vector3(0, 0, angle + 90);
    }

    void Shooting(int entity)
    {
        if (Input.GetMouseButton(0))
        {
            Storage.GetComponent<PlayerControllerCmp>(entity).Gun.shooting = true;
        }
        else
        {
            Storage.GetComponent<PlayerControllerCmp>(entity).Gun.shooting = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Storage.GetComponent<PlayerControllerCmp>(entity).Gun.shooting = false;
        }
    }


    float FindRotateAngle(Vector3 PlayerPos, Vector3 MousePos)
    {
        // вместо y - z
        // ϕ=arccos(cosϕ)
        // 1 градус = 1 радиан / pi * 180
        // (ax ⋅ bx + ay ⋅ by) / ( √(ax^2 + ay^2) ⋅ √(bx^2 + by^2))

        Vector3 A = Vector3.down;
        Vector3 B = MousePos - PlayerPos;


        float denominator = (Mathf.Sqrt(A.x * A.x + A.y * A.y) * Mathf.Sqrt(B.x * B.x + B.y * B.y));
        denominator = denominator == 0 ? 0.0001f : denominator;

        float angle = (A.x * B.x + A.y * B.y) / denominator;

        angle = (float)(Math.Acos(angle) / Math.PI * 180);
        angle = angle * CalculateRotateCoef(A, B);
        return angle;
    }

    int CalculateRotateCoef(Vector3 PlayerPos, Vector3 MousePos)
    {
        // Если есть вектор |AB|, заданный координатами ax,ay,bx,by и точка px,py,
        // ax = 0; ay = 0;
        // (bx - ax) * (py - ay) - (by - ay) * (px - ax)
        // => bx * py - by * px
        // => Forvard.x * TargetPoint.y - Forvard.y * TargetPoint.x

        Vector3 A = Vector3.up;
        Vector3 B = MousePos - PlayerPos;

        if ((0 * B.y - 1 * B.x) >= 0) //влево
            return -1;
        return 1;
    }


}
