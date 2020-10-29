


[System.Serializable]
public abstract class StateBase
{
    protected StateMachineCmp stateMachine;
    protected SMData smData;

    public void Init(StateMachineCmp stateMachine)
    {
        this.stateMachine = stateMachine;
        this.smData = stateMachine.smData;
    }


    public abstract void EnterState();
    public abstract void ExitState();


    public virtual void StateUpdate() { }
    public virtual void StateFixedUpdate() { }
    public virtual void StateLateUpdate() { }

    public virtual bool ShoudSethisState() => false;
}
