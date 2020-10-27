using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

//сделано допущение, что логика храниться на компоненте в угоду сложности и объему кода
public class StateMachineCmp : ComponentBase, ICustomAwake
{
    StateBase previousState;
    StateBase currentState;
    //StateBase nextState;

    public SMData smData;

    [Header("States")]
    public InactiveState inactiveState;
    public FindTargetState findTargetState;
    public FightState fightState;
    public GoBackState goBackState;
    public IdleState idleState;

    

    [Header("Debug")]
    public bool show_gizmos;
    public string cur_state_name;

    public void OnAwake()
    {
        inactiveState.Init(this);
        findTargetState.Init(this);
        fightState.Init(this);
        goBackState.Init(this);
        idleState.Init(this);

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
    }
}

//вся общая дата, которая может понадобиться для состояний (мб сделать всю дату, даже которая есть только на одном состоянии)
[System.Serializable]
public class SMData
{
    public Transform target;
    public Vector3 defaultPos;
    [Min(0), Tooltip("на какой дистанции можно атаковать (красная область)")]
    public float fight_distance;
    [Min(0), Tooltip("на какой дистанции можно идти к цели (зеленая область)")]
    public float vision_distance;
    [Min(0), Tooltip("на какой дистанции устанавливается неактивное состояние (синяя область)")]
    public float active_distance;

    public AIMoveCmp aiMove;
}
