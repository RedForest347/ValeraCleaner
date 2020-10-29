using RangerV;
using UnityEngine;

[System.Serializable]
public class InactiveState : StateBase
{
    [Tooltip("раз во сколько кадров совершается проверка на нахождение цели в области реагирования"), Min(0)]
    public int inactive_sleep_time;
    int cur_frame;

    public override void EnterState()
    {
        smData = stateMachine.smData;

        smData.aiMove = Storage.GetComponent<AIMoveCmp>(stateMachine.entity);
        smData.aiMove.moveMode = AIMoveMode.Stopping;
        smData.aiMove.OnStop += OnStopped;

        cur_frame = 0;
    }

    public override void StateUpdate()
    {
        if (cur_frame++ >= inactive_sleep_time)
        {
            float distance = ((Vector2)(smData.target.transform.position - stateMachine.transform.position)).magnitude;

            if (distance < smData.active_distance)
            {
                stateMachine.SetNewState(stateMachine.idleState);
            }

            cur_frame = 0;
        }
    }

    public override void ExitState()
    {
        smData.aiMove.OnStop -= OnStopped;
    }

    void OnStopped(int ent)
    {
        smData.aiMove.moveMode = AIMoveMode.Sleep;
    }

}
