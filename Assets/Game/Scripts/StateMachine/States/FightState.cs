using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FightState : StateBase
{
    //SMData smData;


    public override void EnterState()
    {
        smData = stateMachine.smData;
        smData.aiMove.OnReached += OnReached;
        smData.aiMove.OnStop += OnStopped;
    }

    public override void StateUpdate()
    {
        float distance = ((Vector2)(smData.target.position - smData.aiMove.transform.position)).magnitude;
        //Debug.Log("distance = " + distance);
        if (distance > smData.fight_distance)
        {
            smData.aiMove.SetTarget(smData.target);
            smData.aiMove.moveMode = AIMoveMode.GoToTarget;
            //Debug.Log("target = " + smData.target + " pos = " + smData.target.transform.position);

            if (distance > smData.vision_distance)
            {
                stateMachine.SetNewState(stateMachine.findTargetState);
            }
        }
        else
        {
            StopMove();
        }
    }

    public override void ExitState()
    {
        smData.aiMove.OnReached -= OnReached;
        smData.aiMove.OnStop -= OnStopped;
    }


    public override bool ShoudSethisState()
    {
        float distance = ((Vector2)(smData.target.position - smData.aiMove.transform.position)).magnitude;

        return distance <= smData.vision_distance;
    }


    void StopMove()
    {
        if (smData.aiMove.moveMode == AIMoveMode.GoToTarget)
        {
            smData.aiMove.moveMode = AIMoveMode.Stopping;
        }
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
