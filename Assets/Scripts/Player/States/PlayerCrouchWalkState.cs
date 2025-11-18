public class PlayerCrouchWalkState : PlayerState
{
    public PlayerCrouchWalkState(PlayerController player, PlayerStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        _player.SetCrouch(true);
        _player.Animator.Play("P_Global_CrouchWalk");
    }

    public override void HandleInput()
    {
        if (!_player.IsCrouching)
        {
            _state.ChangeState(_player.MoveState);
            return;
        }

        if (_player.MoveInput.magnitude < 0.1f)
        {
            _state.ChangeState(_player.CrouchState);
            return;
        }

        if (_player.JumpPressed && _player.IsGrounded())
        {
            _state.ChangeState(_player.JumpState);
            return;
        }
    }

    public override void Update()
    {
        _player.PlayerMovement();
        _player.RotateToCameraDirection();
    }

    public override void Exit()
    {
        _player.SetCrouch(false);
    }
}
