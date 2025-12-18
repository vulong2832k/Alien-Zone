public class PlayerCrouchWalkState : PlayerState
{
    public PlayerCrouchWalkState(PlayerController player, PlayerStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        _player.SetCrouch(true);
        PlayCrouchWalkAnimation();
    }

    public override void OnGunChanged()
    {
        PlayCrouchWalkAnimation();
    }

    private void PlayCrouchWalkAnimation()
    {
        if (_player.IsMovingBackward)
        {
            _player.PlayGunBasedAnimation(
                "P_Global_CrouchWalkback",
                "P_Pistol_CrouchWalkback",
                "P_Rifle_CrouchWalkback"
            );
        }
        else
        {
            _player.PlayGunBasedAnimation(
                "P_Global_CrouchWalk",
                "P_Pistol_CrouchWalk",
                "P_Rifle_CrouchWalk"
            );
        }
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
        if (_player.IsActionLocked) return;

        _player.PlayerMovement();

        if (!_player.IsMovingBackward)
            _player.RotateToCameraDirection();

        PlayCrouchWalkAnimation();
    }

    public override void Exit()
    {
        _player.SetCrouch(false);
    }
}
