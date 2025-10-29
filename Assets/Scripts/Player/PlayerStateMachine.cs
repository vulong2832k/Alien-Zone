using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startState)
    {
        this.CurrentState = startState;
        startState.Enter();
    }
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
    }
}
