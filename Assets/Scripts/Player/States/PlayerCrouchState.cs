public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(PlayerController player, PlayerStateMachine state)
        : base(player, state) { }

    public override void HandleInput()
    {
        if (!_player.IsCrouching)
        {
            _state.ChangeState(_player.IdleState);
        }
    }

    public override void Enter()
    {
        _player.Animator.Play("Crouch");
        _player.SetCrouch(true);
    }

    public override void Exit()
    {
        _player.SetCrouch(false);
    }
}
