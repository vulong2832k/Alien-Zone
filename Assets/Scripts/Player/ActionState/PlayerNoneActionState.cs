using UnityEngine;

public class PlayerNoneActionState : PlayerActionState
{
    public PlayerNoneActionState(PlayerController player, PlayerActionStateMachine state)
        : base(player, state) { }

    public override void Update()
    {
        if (_player.Gun == null) return;

        if (_player.ReloadPressed && _player.Gun.CanReload())
        {
            _actionState.ChangeState(_player.ReloadState);
            return;
        }

        // Auto reload nếu hết đạn
        if (_player.Gun.NeedsReload())
        {
            _actionState.ChangeState(_player.ReloadState);
        }
    }
}
