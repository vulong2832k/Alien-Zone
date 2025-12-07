using System.Collections;
using UnityEngine;

public class PlayerReloadState : PlayerActionState
{
    public PlayerReloadState(PlayerController player, PlayerActionStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        base.Enter();
        _player.IsActionLocked = true;

        if (_player.Gun == null)
        {
            _actionState.ChangeState(_player.NoneActionState);
            return;
        }

        _player.Gun.BlockFire = true;
        string anim = GetReloadAnimation();
        _player.Animator.Play(anim);

        _player.StartCoroutine(ReloadRoutine());
    }


    public override void Exit()
    {
        _player.Gun.BlockFire = false;
        _player.IsActionLocked = false;
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
        yield return null;
        yield return new WaitForEndOfFrame();

        AnimatorStateInfo info = _player.Animator.GetCurrentAnimatorStateInfo(0);
        while (!info.IsName(GetReloadAnimation()))
        {
            info = _player.Animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        float animTime = info.length;

        yield return new WaitForSeconds(animTime);

        _player.Gun.DoReload();
        _actionState.ChangeState(_player.NoneActionState);
    }
}
