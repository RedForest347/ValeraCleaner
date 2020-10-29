using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindTargetState : StateBase
{
    [Min(0), Tooltip("время преследования, секунды")]
    public float pursued_time;
    float cur_time;


    public override void EnterState()
    {
        smData = stateMachine.smData;

        smData.aiMove.moveMode = AIMoveMode.GoToTarget;
        smData.aiMove.OnReached += OnReached;
        smData.aiMove.OnStop += OnStopped;

        cur_time = 0;
    }

    public override void StateUpdate()
    {
        float distance = ((Vector2)(smData.target.position - smData.aiMove.transform.position)).magnitude;

        if (cur_time > pursued_time)
        {
            stateMachine.SetNewState(stateMachine.goBackState);
        }
        else if (distance < smData.vision_distance)
        {
            stateMachine.SetNewState(stateMachine.fightState);
        }

        cur_time += Time.deltaTime;
    }


    public override void ExitState()
    {
        smData.aiMove.OnReached -= OnReached;
        smData.aiMove.OnStop -= OnStopped;
    }

    void OnReached(int ent)
    {
        smData.aiMove.moveMode = AIMoveMode.Stopping;
    }

    void OnStopped(int ent)
    {
        smData.aiMove.moveMode = AIMoveMode.Sleep;
    }

}
