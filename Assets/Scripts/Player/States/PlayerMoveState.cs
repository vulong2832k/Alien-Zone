using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        _player.MoveSpeed = _player.DefaultMoveSpeed;
        PlayMoveAnimation();
    }
    public override void OnGunChanged()
    {
        PlayMoveAnimation();
    }
    private void PlayMoveAnimation()
    {
        if (_player.IsMovingBackward)
        {
            _player.PlayGunBasedAnimation(
                "P_Global_RunBack",
                "P_Pistol_RunBack",
                "P_Rifle_RunBack"
            );
        }
        else
        {
            _player.PlayGunBasedAnimation(
                "P_Global_Run",
                "P_Pistol_Run",
                "P_Rifle_Run"
            );
        }
    }
    public override void HandleInput()
    {
        if (_player.IsActionLocked) return;

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

        if (!_player.IsMovingBackward)
            _player.RotateToCameraDirection();
    }
    
}
