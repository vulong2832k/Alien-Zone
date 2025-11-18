public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(PlayerController player, PlayerStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        _player.SetCrouch(true);
        _player.Animator.Play("P_Global_Crouch");
        _player.MoveSpeed = _player.CrouchSppeed;
    }

    public override void HandleInput()
    {
        if (!_player.IsCrouching)
        {
            _state.ChangeState(_player.IdleState);
            return;
        }

        if (_player.MoveInput.magnitude >= 0.1f)
        {
            _state.ChangeState(_player.CrouchWalkState);
        }
    }

    public override void Exit()
    {
        _player.SetCrouch(false);
        _player.MoveSpeed = _player.DefaultMoveSpeed;
    }
}
