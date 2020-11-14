using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;
using UnityEditor;

//сделано допущение, что логика храниться на компоненте в угоду сложности и объему кода
[Component("AI/StateMachine")]
public class StateMachineCmp : ComponentBase, ICustomAwake
{
    StateBase previousState;
    StateBase currentState;
    //StateBase nextState;

    [Header("Data")]
    public SMData smData;

    [Header("States")]
    public InactiveState inactiveState;
    public FindTargetState findTargetState;
    public FightState fightState;
    public GoBackState goBackState;
    public IdleState idleState;

    [Header("Debug")]
    public bool show_gizmos;
    public bool show_gizmos_alternative;
    public string cur_state_name;

    public void OnAwake()
    {
        InitStates(new StateBase[]
        {
                inactiveState,
                findTargetState,
                fightState,
                goBackState,
                idleState
        });


        smData.aiMove = Storage.GetComponent<AIMoveCmp>(entity);
        smData.defaultPos = transform.position;

        InitFirstState(idleState);
    }


    public void StateUpdate()
    {
        currentState.StateUpdate();
    }

    public void StateFixedUpdate()
    {
        currentState.StateFixedUpdate();
    }

    public void StateLateUpdate()
    {
        currentState.StateLateUpdate();
    }


    void InitFirstState(StateBase state)
    {
        state.EnterState();
        previousState = null;
        SetCurrentState(state);
        //nextState = null;
    }

    void InitStates(StateBase[] states)
    {
        for (int i = 0; i < states.Length; i++)
        {
            states[i].Init(this);
        }
    }

    public void SetNewState(StateBase newState)
    {
        currentState.ExitState();
        newState.EnterState();

        previousState = currentState;
        SetCurrentState(newState);
    }

    void SetCurrentState(StateBase state)
    {
        currentState = state;
        cur_state_name = currentState.GetType().Name;
    }



    private void OnDrawGizmos()
    {
        if (show_gizmos)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawWireSphere(transform.position, smData.fight_distance);

            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawWireSphere(transform.position, smData.vision_distance);

            Gizmos.color = new Color(0, 0, 1, 0.3f);
            Gizmos.DrawWireSphere(transform.position, smData.active_distance);
        }
        if (show_gizmos_alternative)
        {
            HandlesExtension.DrawZone(transform, 360, smData.fight_distance, new Color(1, 0, 0, 0.13f));
            HandlesExtension.DrawZone(transform, 360, smData.vision_distance, new Color(0, 0.7f, 0, 0.08f));
            HandlesExtension.DrawZone(transform, 360, smData.active_distance, new Color(0, 0, 0.5f, 0.06f));
        }
    }
}

//вся общая дата, которая может понадобиться для состояний (мб сделать всю дату, даже которая есть только на одном состоянии)
[System.Serializable]
public class SMData
{
    public Transform target;

    [Min(0), Tooltip("на какой дистанции можно атаковать (красная область)")]
    public float fight_distance;
    [Min(0), Tooltip("на какой дистанции можно идти к цели (зеленая область)")]
    public float vision_distance;
    [Min(0), Tooltip("на какой дистанции устанавливается неактивное состояние (синяя область)")]
    public float active_distance;

    [HideInInspector]
    public Vector3 defaultPos;
    [HideInInspector]
    public AIMoveCmp aiMove;
}
