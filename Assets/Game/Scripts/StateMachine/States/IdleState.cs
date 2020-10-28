using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleState : StateBase
{
    public float time_befor_walk;
    public float walk_distance;
    float cur_walk_time;
    bool walking;
    

    public override void EnterState()
    {
        smData = stateMachine.smData;
        smData.aiMove.OnReached += OnReached;
        smData.aiMove.OnStop += OnStopped;
        smData.aiMove.moveMode = AIMoveMode.Stopping;

        cur_walk_time = 0;
        walking = false;
    }

    public override void StateUpdate()
    {
        if (CheckSetNewState())
            return;

        if (walking)
            return;

        if (cur_walk_time >= time_befor_walk)
        {
            float num = walk_distance;
            Vector3 newTarget = smData.defaultPos + new Vector3(Random.Range(-num, num), Random.Range(-num, num), 0);
            smData.aiMove.SetTarget(newTarget);
            smData.aiMove.moveMode = AIMoveMode.GoToTarget;
            walking = true;
        }


        cur_walk_time += Time.deltaTime;
    }


    public override void ExitState() 
    {
        smData.aiMove.OnReached -= OnReached;
        smData.aiMove.OnStop -= OnStopped;
    }

    bool CheckSetNewState()
    {
        float distance = ((Vector2)(smData.target.transform.position - stateMachine.transform.position)).magnitude;

        if (distance <= smData.vision_distance)
        {
            stateMachine.SetNewState(stateMachine.fightState);
            return true;
        }

        if (distance > smData.active_distance)
        {
            stateMachine.SetNewState(stateMachine.inactiveState);
            return true;
        }
        return false;
    }

    void OnReached(int ent)
    {
        smData.aiMove.moveMode = AIMoveMode.Stopping;
    }

    void OnStopped(int ent)
    {
        smData.aiMove.moveMode = AIMoveMode.Sleep;
        cur_walk_time = 0;
        walking = false;
    }
}
