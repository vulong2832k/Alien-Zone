public class PlayerActionStateMachine
{
    public PlayerActionState CurrentState { get; private set; }
    public bool IsPlayStateActive = false;

    public void Initialize(PlayerActionState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerActionState newState, bool isPriority = false)
    {
        if (CurrentState == newState) return;
        if (IsPlayStateActive && !isPriority) return;

        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();

        IsPlayStateActive = isPriority;
    }
    public bool IsBusy()
    {
        return !(CurrentState is PlayerNoneActionState);
    }
}
