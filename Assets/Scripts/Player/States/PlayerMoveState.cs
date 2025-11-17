using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player, PlayerStateMachine state) : base(player, state)
    {
    }
    public override void HandleInput()
    {
        if (_player.MoveInput.magnitude < 0.1f)
        {
            _state.ChangeState(_player.IdleState);
        }
        if (_player.JumpPressed)
        {
            _state.ChangeState(_player.JumpState);
        }
        if (_player.IsCrouching)
        {
            _state.ChangeState(_player.CrouchState);
        }
    }
    public override void Update()
    {
        _player.PlayerMovement();
        _player.RotateToCameraDirection();
        _player.Animator.Play("Move");
    }
}
