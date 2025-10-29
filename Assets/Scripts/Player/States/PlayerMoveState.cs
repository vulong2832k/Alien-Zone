using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player, PlayerStateMachine state) : base(player, state)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _player.PlayerMovement();

        if (_player.IsCrouching)
        {
            _state.ChangeState(_player.CrouchState);
            return;
        }
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _player.MoveSpeed = _player.MoveWalking;
        }
        else
        {
            _player.MoveSpeed = _player.DefaultMoveSpeed;
        }
        _player.PlayerMovement();

        if(_player.MoveInput.magnitude <= 0.1f)
        {
            _state.ChangeState(_player.IdleState);
        }
    }
}
