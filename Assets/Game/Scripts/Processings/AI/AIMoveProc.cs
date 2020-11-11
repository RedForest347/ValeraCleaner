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
    Group AIMoveGroup = Group.Create(new ComponentsList<AIMoveCmp, MoverCmp>());

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
            MoveToTarget(AI);
        }
    }



    void MoveToTarget(int ai)
    {
        AIMoveCmp aiMove = Storage.GetComponent<AIMoveCmp>(ai);
        MoverCmp mover = Storage.GetComponent<MoverCmp>(ai);

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
            AddForce(aiMove, mover);
            SetRotation(aiMove, mover);
            DebugPath(aiMove);
        }
        else if (aiMove.moveMode == AIMoveMode.Stopping)
        {
            StopAtTheTarget(aiMove, mover);
        }
    }

    void SearchNewPath(AIMoveCmp aiMove)
    {
        //Debug.Log("SearchNewPath");
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

    void AddForce(AIMoveCmp aiMove, MoverCmp mover)
    {
        Vector3 target_pos = aiMove.current_move_point;
        mover.AddDirection(target_pos - aiMove.transform.position);
    }

    void SetRotation(AIMoveCmp aiMove, MoverCmp mover)
    {
        Vector2 start = (Vector3)Vector2.right;
        Vector2 end = aiMove.current_move_point - (Vector2)aiMove.transform.position;

        float cur_rotation = Vector2.SignedAngle(start, end);
        mover.SetRotation(cur_rotation);
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

    void StopAtTheTarget(AIMoveCmp aiMove, MoverCmp mover)
    {
        Vector2 toStop = -aiMove.rb.velocity;

        if (toStop.magnitude > mover.MaxSpeed)
        {
            //toStop = toStop * mover.MaxSpeed;
            mover.AddDirection(-toStop); 
        }
        else
        {
            aiMove.rb.velocity = Vector2.zero; /// !!!!!!!!!!!!!!!!!!!!!!!!!! есть необходимость задавать кастомную скорость
            aiMove.OnStop(aiMove.entity); 
        }
    }

    bool IsNearby(AIMoveCmp aiMove)
    {
        Vector2 start = aiMove.transform.position;
        List<GraphNode> path = aiMove.aILerp.GetPathCustom().path;
        Vector2 fin = aiMove.target;

        aiMove.current_distance_to_target = (start - fin).magnitude; // for debug
        aiMove.current_distance_to_nearby_point = (start - aiMove.current_move_point).magnitude; // for debug

        return (start - fin).magnitude < aiMove.nearby_distance;
    }

}
