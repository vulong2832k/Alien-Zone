using UnityEngine;

public abstract class PlayerActionState
{
    protected PlayerController _player;
    protected PlayerActionStateMachine _actionState;

    public PlayerActionState(PlayerController player, PlayerActionStateMachine actionState)
    {
        this._player = player;
        this._actionState = actionState;
    }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
