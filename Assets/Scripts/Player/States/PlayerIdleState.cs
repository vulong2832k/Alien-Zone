using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        _player.Animator.Play("P_Global_Idle");
    }

    public override void HandleInput()
    {
        if (_player.JumpPressed && _player.IsGrounded())
        {
            _state.ChangeState(_player.JumpState);
            return;
        }

        if (_player.MoveInput.magnitude > 0.1f)
        {
            _state.ChangeState(_player.MoveState);
            return;
        }

        if (_player.IsCrouching)
        {
            _state.ChangeState(_player.CrouchState);
            return;
        }
    }
    public override void Update()
    {
        base.Update();
        _player.RotateToCameraDirection();
    }
}
