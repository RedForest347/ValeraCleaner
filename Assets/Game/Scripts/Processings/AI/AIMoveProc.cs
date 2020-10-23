using System.Collections;
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

public class AIMoveProc : ProcessingBase, ICustomUpdate, ICustomStart
{
    Group AIMoveGroup = Group.Create(new ComponentsList<AIMoveCmp>());

    public void OnStart()
    {
        if (Object.FindObjectOfType<AstarPath>() == null)
        {
            Debug.LogWarning("добавте на сцену объект с компонентом Pathfinding (AstarPath)");
        }
    }

    public void CustomUpdate()
    {
        //Debug.Log("AIMoveGroup.entities_count = " + AIMoveGroup.entities_count);
        foreach (int AI in AIMoveGroup)
        {
            CustomMoveToTarget(AI);
        }
    }



    void CustomMoveToTarget(int AI)
    {
        AIMoveCmp aiMove = Storage.GetComponent<AIMoveCmp>(AI);

        SearchNewPath(aiMove);

        if (!ShouldContinue(aiMove))
            return;

        if (CheckNearby(aiMove))
        {
            StopAtTheTarget(aiMove);

            return;
        }

        

        FindCurrentMovePoint(aiMove);
        AddForce(aiMove);
        DebugPath(aiMove);
    }

    void SearchNewPath(AIMoveCmp aiMove)
    {
        if (aiMove.target == null)
            return;

        aiMove.aILerp.SearchNewPathCustom(aiMove.target.transform.position);
    }

    bool CheckNearby(AIMoveCmp aiMove)
    {
        aiMove.finished = IsNearby(aiMove);
        return aiMove.finished;
    }


    bool ShouldContinue(AIMoveCmp aiMove)
    {

        if (aiMove.target == null)
        {
            //Debug.LogWarning("target not assign");
            return false;
        }

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
        else if (aiMove.path_length == 0)
        {
            aiMove.current_move_point = (Vector3)path[0].position;
        }
    }

    void AddForce(AIMoveCmp aiMove)
    {
        Vector3 target_pos = aiMove.current_move_point;

        aiMove.rb.AddForce(((Vector2)(target_pos - aiMove.transform.position)).normalized * aiMove.acceleration);

        if (aiMove.rb.velocity.magnitude > aiMove.max_speed)
            aiMove.rb.velocity = aiMove.rb.velocity.normalized * aiMove.max_speed;
    }

    void DebugPath(AIMoveCmp aiMove)
    {
        Vector3 target_pos = aiMove.current_move_point;
        Debug.DrawLine(aiMove.transform.position, target_pos, Color.red);
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
        }
    }

    bool IsNearby(AIMoveCmp aiMove)
    {
        Vector2 start = aiMove.transform.position;
        List<GraphNode> path = aiMove.aILerp.GetPathCustom().path;
        Vector2 fin = (Vector3)path[path.Count - 1].position;

        aiMove.distance_to_target = (start - fin).magnitude; //debug


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
