﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using Pathfinding;


/// <summary>
/// как пользоваться:
///     просто вешаешь на Entuty компонент AIMoveCmp (остальное вешается само)
///     добавляешь данный процессинг в стартер
///     на сцене должен быть Pathfinder (класс называется AstarPath, но имя компонента изменено 
///     для инспектора и в инспекторе оно будет именно Pathfinder), причем только один
/// </summary>
/// 
/// <optimization>
/// обновлять путь только при перестроении карты проходимости (даст внушительный прирост)
/// проходить по ограниченному количеству сущностей за кадр (1- 30, следующий кадр 31 -60, 61 - 25 и т.д.)
/// </optimization>

public class AIMoveProc : ProcessingBase, ICustomFixedUpdate, ICustomStart
{
    Group AIMoveGroup = Group.Create(new ComponentsList<AIMoveCmp>());

    public void OnStart()
    {
        if (Object.FindObjectOfType<AstarPath>() == null)
        {
            Debug.LogWarning("добавте на сцену объект с компонентом Pathfinding (AstarPath)");
        }
    }

    public void CustomFixedUpdate()
    {
        //Debug.Log("AIMoveGroup.entities_count = " + AIMoveGroup.entities_count);
        foreach (int AI in AIMoveGroup)
        {
            CustomMoveToTarget(AI);
        }
    }



    void CustomMoveToTarget(int ai)
    {
        AIMoveCmp aiMove = Storage.GetComponent<AIMoveCmp>(ai);

        if (aiMove.moveMode == AIMoveMode.GoToTarget)
        {
            SearchNewPath(aiMove);

            if (!EverythingIsFine(aiMove))
            {
                Debug.Log("not fine");
                return;
            }

            if (CheckNearby(aiMove))
            {
                //Debug.Log("Nearby");
                aiMove.finished = true;
                //StopAtTheTarget(aiMove);
                Debug.Log("CheckNearby");
                return;
            }



            FindCurrentMovePoint(aiMove);
            AddForce(aiMove);
            //if (aiMove.draw_gizmos)
            //{
                DebugPath(aiMove);
            //}
        }
        else if (aiMove.moveMode == AIMoveMode.Stopping)
        {
            StopAtTheTarget(aiMove);
        }
    }

    void SearchNewPath(AIMoveCmp aiMove)
    {
        /*if (aiMove.target == null)
            return;*/
        Debug.Log("SearchNewPath");
        aiMove.aILerp.SearchNewPathCustom(aiMove.target);
    }

    bool CheckNearby(AIMoveCmp aiMove)
    {
        return IsNearby(aiMove);

        /*aiMove.finished = IsNearby(aiMove);
        return aiMove.finished;*/
    }


    bool EverythingIsFine(AIMoveCmp aiMove)
    {
        /*if (aiMove.target == null)
        {
            //Debug.LogWarning("target not assign");
            return false;
        }*/

        if (aiMove.aILerp.GetPathCustom() == null)
        {
            //Debug.LogWarning("GetPathCustom() == null");
            return false;
        }

        return true;
    }

    void FindCurrentMovePoint(AIMoveCmp aiMove)
    {
        List<GraphNode> path = aiMove.aILerp.GetPathCustom().path;
        aiMove.path_length = path.Count;
        aiMove.finished = false;

        if (aiMove.path_length > 1)
        {
            aiMove.current_move_point = (Vector3)path[1].position;
        }
        else if (aiMove.path_length == 1)
        {
            aiMove.current_move_point = (Vector3)path[0].position;
        }
        else if (aiMove.path_length == 0)
        {
            Debug.LogError("длинна найденного пути ноль");
        }
    }

    void AddForce(AIMoveCmp aiMove)
    {
        Vector3 target_pos = aiMove.current_move_point;

        //Debug.Log("normalized = " + ((Vector2)(target_pos - aiMove.transform.position)).normalized + " aiMove.acceleration = " + aiMove.acceleration);
        //Debug.Log("acceleration = " + (((Vector2)(target_pos - aiMove.transform.position)).normalized * aiMove.acceleration));
        //Debug.Log("AddForce " + (((Vector2)(target_pos - aiMove.transform.position)).normalized * aiMove.acceleration));
        aiMove.rb.AddForce(((Vector2)(target_pos - aiMove.transform.position)).normalized * aiMove.acceleration);

        if (aiMove.rb.velocity.magnitude > aiMove.max_speed)
            aiMove.rb.velocity = aiMove.rb.velocity.normalized * aiMove.max_speed;
    }

    void DebugPath(AIMoveCmp aiMove)
    {
        if (aiMove.draw_gizmos)
        {
            Vector3 neaby_target_pos = aiMove.current_move_point;
            Vector3 final_target = aiMove.target;
            Debug.DrawLine(aiMove.transform.position, neaby_target_pos, Color.red);
            Debug.DrawLine(aiMove.transform.position, final_target, Color.blue);
        }
        aiMove.cur_speed = ((Vector2)aiMove.rb.velocity).magnitude;
    }

    void StopAtTheTarget(AIMoveCmp aiMove)
    {
        Vector2 toStop = -aiMove.rb.velocity;

        if (toStop.magnitude > aiMove.max_speed)
        {
            toStop = toStop * aiMove.max_speed;
            aiMove.rb.AddForce(toStop);
        }
        else
        {
            aiMove.rb.velocity = Vector2.zero;
            aiMove.OnStop(aiMove.entity);
        }
    }



    //float previous_distance;
    bool IsNearby(AIMoveCmp aiMove)
    {
        Vector2 start = aiMove.transform.position;
        List<GraphNode> path = aiMove.aILerp.GetPathCustom().path;
        Vector2 fin = (Vector3)path[path.Count - 1].position;

        aiMove.distance_to_target = (start - fin).magnitude; //debug

        //Debug.Log("start = " + start + " fin = " + fin + " target = " + aiMove.target);
        return (start - fin).magnitude < aiMove.nearby_distance;


        /*
          if ((start - fin).magnitude < aiMove.nearby_distance)
            if (InLineOfSight(aiMove, start, fin))
                return true;
        return false;
         */

    }


    /*bool InLineOfSight(AIMoveCmp aiMove, Vector2 RayStart, Vector2 RayFin) // в зоне прямой видимости. дописать
    {
        RaycastHit2D[] raycast = Physics2D.RaycastAll(RayStart, RayFin - RayStart, aiMove.nearby_distance);

        bool _out = raycast.collider.gameObject == aiMove.target.gameObject;
        Debug.Log("out = " + _out + " collider = " + raycast.collider.gameObject + " target = " + aiMove.target.gameObject);
        return _out;
    }*/
}

/*public enum NearbyState
{
    nearby,
    not_nearby
}*/
