using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine state) : base(player, state)
    {
    }

    public override void HandleInput()
    {
        if (_player.MoveInput.magnitude > 0.1f)
        {
            _state.ChangeState(_player.MoveState);
        }
        if (_player.JumpPressed)
        {
            _state.ChangeState(_player.JumpState);
        }
        else if (_player.IsCrouching)
        {
            _state.ChangeState(_player.CrouchState);
        }
    }
    public override void Enter()
    {
        _player.Animator.Play("P_Global_Idle");
    }
}
