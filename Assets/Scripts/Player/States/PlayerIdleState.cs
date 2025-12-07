using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        _player.PlayGunBasedAnimation(
            "P_Global_Idle",
            "P_Pistol_Idle",
            "P_Rifle_Idle"
        );
    }
    public override void OnGunChanged()
    {
        _player.PlayGunBasedAnimation(
            "P_Global_Idle",
            "P_Pistol_Idle",
            "P_Rifle_Idle"
        );
    }
    public override void HandleInput()
    {
        _player.RotateToCameraDirection();

        if (_player.IsActionLocked) return;

        if (_player.ReloadPressed)
        {
            _player.ReloadPressed = false;
            _player.ActionStateMachine.ChangeState(_player.ReloadState);
            return;
        }

        if (_player.JumpPressed && _player.IsGrounded())
        {
            _state.ChangeState(_player.JumpState);
            return;
        }

        if (_player.MoveInput.magnitude > 0.1f)
        {
            if (_player.WantWalk)
                _state.ChangeState(_player.WalkState);
            else
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
        _player.ReloadPressed = false;
    }
}
