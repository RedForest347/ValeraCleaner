using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using Pathfinding;


/// <summary>
/// как пользоваться:
///     просто вешаешь на Entity компонент AIMoveCmp, Rigitbody (остальное вешается само)
///     добавляешь данный процессинг в стартер
///     на сцене должен быть Pathfinder (класс называется AstarPath, но имя компонента изменено 
///     для инспектора и в инспекторе оно будет именно Pathfinder), причем только один
/// </summary>
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
                return;
            }

            if (IsNearby(aiMove))
            {
                aiMove.finished = true;
                return;
            }

            FindCurrentMovePoint(aiMove);
            AddForce(aiMove);
            DebugPath(aiMove);
        }
        else if (aiMove.moveMode == AIMoveMode.Stopping)
        {
            StopAtTheTarget(aiMove);
        }
    }

    void SearchNewPath(AIMoveCmp aiMove)
    {
        aiMove.aILerp.SearchNewPathCustom(aiMove.target);
    }

    bool EverythingIsFine(AIMoveCmp aiMove)
    {
        if (aiMove.aILerp.GetPathCustom() == null)
        {
            return false;
        }

        return true;
    }

    void FindCurrentMovePoint(AIMoveCmp aiMove)
    {
        List<GraphNode> path = aiMove.aILerp.Path.path;
        int path_length = path.Count;
        aiMove.path_length = path_length;
        aiMove.finished = false;

        if (path_length > 1)
        {
            aiMove.current_move_point = (Vector3)path[1].position;
        }
        else if (path_length == 1)
        {
            aiMove.current_move_point = aiMove.target;
        }
        else if (path_length == 0)
        {
            Debug.LogError("длинна найденного пути ноль");
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

    bool IsNearby(AIMoveCmp aiMove)
    {
        Vector2 start = aiMove.transform.position;
        List<GraphNode> path = aiMove.aILerp.GetPathCustom().path;
        //Vector2 fin = (Vector3)path[path.Count - 1].position;
        Vector2 fin = aiMove.target;
        Debug.Log("start =" + start + " fin = " + fin);

        aiMove.distance_to_target = (start - fin).magnitude; //debug

        return (start - fin).magnitude < aiMove.nearby_distance;
    }

}
