using System.Collections;
using UnityEngine;

public class PlayerReloadState : PlayerActionState
{
    public PlayerReloadState(PlayerController player, PlayerActionStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        _player.Gun.BlockFire = true;
        string anim = GetReloadAnimation();
        _player.Animator.Play(anim);
        _player.StartCoroutine(ReloadRoutine());
    }

    public override void Exit()
    {
        _player.Gun.BlockFire = false;
    }

    private string GetReloadAnimation()
    {
        if (_player.StateMachine.CurrentState == _player.CrouchState)
            return "P_Global_CrouchReload";
        if (_player.StateMachine.CurrentState == _player.MoveState)
            return "P_Global_RunReload";
        if (_player.StateMachine.CurrentState == _player.WalkState)
            return "P_Global_WalkReload";
        return "P_Global_IdleReload";
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_player.Gun.GunAttributes.Reload);

        _player.Gun.DoReload();

        _actionState.ChangeState(_player.NoneActionState);
    }
}
