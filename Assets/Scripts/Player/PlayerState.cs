using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _player;
    protected PlayerStateMachine _state;

    public PlayerState(PlayerController player, PlayerStateMachine state)
    {
        this._player = player;
        this._state = state;
    }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
