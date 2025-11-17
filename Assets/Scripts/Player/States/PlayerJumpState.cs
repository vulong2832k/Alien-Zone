using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        base.Enter();
        if (_player.JumpPressed && _player.IsGrounded())
        {
            _player.PlayerRb.AddForce(Vector3.up * _player.JumpForce, ForceMode.Impulse);
        }
        _player.ConsumeJumpPressed();

        if (_player.Animator != null) _player.Animator.Play("P_Global_Jump");
    }

    public override void Update()
    {
        base.Update();
        if (!_player.IsGrounded())
        {
            _state.ChangeState(_player.CrouchState);
        }
    }
}
