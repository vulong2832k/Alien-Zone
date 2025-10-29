using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine state) : base(player, state)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_player.MoveInput.magnitude > 0.1f)
        {
            _state.ChangeState(_player.MoveState);
        }
        else if (_player.IsCrouching)
        {
            _state.ChangeState(_player.CrouchState);
        }
    }
}
