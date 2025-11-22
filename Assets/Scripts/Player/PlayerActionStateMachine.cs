using UnityEngine;
public class PlayerActionStateMachine
{
   
    public PlayerActionState CurrentState;

    public void Initialize(PlayerActionState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerActionState newState)
    {
        Debug.Log("ChangeActionState: " + newState.GetType().Name);
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

}
