using UnityEngine;

public class PlayerNoneActionState : PlayerActionState
{
    public PlayerNoneActionState(PlayerController player, PlayerActionStateMachine state)
        : base(player, state) { }

    public override void Update()
    {
        if (_player.Gun == null)
            return;

        if (_player.ReloadPressed)
        {
            _player.ReloadPressed = false;
            _actionState.ChangeState(_player.ReloadState);
            return;
        }
    }
}
