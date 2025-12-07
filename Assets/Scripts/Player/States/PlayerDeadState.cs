using System.Collections;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerController player, PlayerStateMachine state)
        : base(player, state) { }

    public override void Enter()
    {
        if (_player.PlayerRb != null)
        {
            _player.PlayerRb.linearVelocity = Vector3.zero;
            _player.PlayerRb.isKinematic = true;
        }
        _player.ActionStateMachine.CurrentState?.Exit();

        if (_player.Animator != null)
        {
            _player.Animator.Play("P_Global_Death");
        }
        _player.StartCoroutine(DisablePlayerAfterDelay(3f));
    }

    public override void Exit()
    {
        if (_player.PlayerRb != null)
            _player.PlayerRb.isKinematic = false;
    }

    public override void HandleInput()
    {
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }
    private IEnumerator DisablePlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _player.gameObject.SetActive(false);
    }
}
