using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        _player.Animator.Play("P_Global_Run");
        _player.MoveSpeed = _player.DefaultMoveSpeed;
    }

    public override void HandleInput()
    {
        if (_player.JumpPressed && _player.IsGrounded())
        {
            _state.ChangeState(_player.JumpState);
            return;
        }

        if (_player.IsCrouching)
        {
            if (_player.MoveInput.magnitude > 0.1f)
                _state.ChangeState(_player.CrouchWalkState);
            else
                _state.ChangeState(_player.CrouchState);
            return;
        }

        if (_player.WantWalk)
        {
            _state.ChangeState(_player.WalkState);
            return;
        }

        if (_player.MoveInput.magnitude < 0.1f)
        {
            _state.ChangeState(_player.IdleState);
            return;
        }
    }

    public override void Update()
    {
        _player.PlayerMovement();
        _player.RotateToCameraDirection();
    }
}
