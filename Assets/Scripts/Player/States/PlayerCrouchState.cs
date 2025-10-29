using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(PlayerController player, PlayerStateMachine state) : base(player, state)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _player.IsCrouching = true;
        _player.TweenCrouch(true);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_player.IsCrouching)
        {
            _state.ChangeState(_player.IdleState);
        }
    }
    public override void Exit()
    {
        _player.IsCrouching = false;
        _player.TweenCrouch(false);
    }
}
