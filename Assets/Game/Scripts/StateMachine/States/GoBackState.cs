using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoBackState : StateBase
{
    public override void EnterState()
    {
        smData.aiMove.OnReached += OnReached;
        smData.aiMove.OnStop += OnStopped;
        smData.aiMove.SetTarget(smData.defaultPos);
        smData.aiMove.moveMode = AIMoveMode.GoToTarget;
    }

    public override void StateUpdate()
    {
        float distance = ((Vector2)(smData.target.position - smData.aiMove.transform.position)).magnitude;

        if (distance < smData.vision_distance)
        {
            stateMachine.SetNewState(stateMachine.fightState);
        }
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
        stateMachine.SetNewState(stateMachine.idleState);
    }
}
