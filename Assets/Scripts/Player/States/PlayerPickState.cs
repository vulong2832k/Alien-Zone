using System.Collections;
using UnityEngine;

public class PlayerPickState : PlayerState
{
    public PlayerPickState(PlayerController player, PlayerStateMachine state) : base(player, state) { }

    public override void Enter()
    {
        base.Enter();
        _player.Animator.Play("P_Global_Pick");
        _player.CurrentInteractable?.Interact(_player);
        _player.StartCoroutine(BackToIdle());
    }
    private IEnumerator BackToIdle()
    {
        yield return new WaitForSeconds(2f);
        _state.ChangeState(_player.IdleState);
    }
}
