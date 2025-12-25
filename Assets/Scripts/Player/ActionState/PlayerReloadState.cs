using System.Collections;
using UnityEngine;

public class PlayerReloadState : PlayerActionState
{
    private Coroutine _reloadCoroutine;
    private GunController _reloadGun;

    public PlayerReloadState(PlayerController player, PlayerActionStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        base.Enter();

        if (_player.Gun == null || !_player.Gun.CanReload())
        {
            _actionState.ChangeState(_player.NoneActionState);
            return;
        }
        _player.IsActionLocked = true;
        _reloadGun = _player.Gun;
        _player.Gun.BlockFire = true;

        WeaponEvents.OnWeaponChanged += OnWeaponChanged;

        string anim = GetReloadAnimation();
        _player.Animator.Play(anim);

        _reloadCoroutine = _player.StartCoroutine(ReloadRoutine());
    }


    public override void Exit()
    {
        WeaponEvents.OnWeaponChanged -= OnWeaponChanged;

        if (_reloadGun != null)
            _reloadGun.BlockFire = false;

        _reloadGun = null;

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

        if (_reloadGun == null || _player.Gun != _reloadGun)
            yield break;

        _reloadGun.DoReload();
        _actionState.ChangeState(_player.NoneActionState);
    }
    private void OnWeaponChanged(GunController newGun)
    {
        if (newGun != _reloadGun)
        {
            CancelReload();
        }
    }
    private void CancelReload()
    {
        if (_reloadCoroutine != null)
        {
            _player.StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }

        if (_reloadGun != null)
            _reloadGun.BlockFire = false;

        _reloadGun = null;

        _actionState.ChangeState(_player.NoneActionState);
    }

}
