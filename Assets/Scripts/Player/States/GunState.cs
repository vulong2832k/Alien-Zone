public abstract class GunState
{
    protected PlayerController _player;
    protected GunStateMachine _state;

    public GunState(PlayerController player, GunStateMachine state)
    {
        this._player = player;
        this._state = state;
    }
    public virtual void Enter() { }
    public virtual void LogicUpdate() { }
    public virtual void Exit() { }
}
