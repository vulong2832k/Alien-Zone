using UnityEngine;

public class EnemyHurtState : EnemyState
{
    private bool _hasHurtAnim;

    public EnemyHurtState(EnemyController enemy, EnemyStateMachine state) : base(enemy, state)
    {
        _hasHurtAnim = enemy.HasAnimation("E_Hurt");
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.StopMoving();

        if (_hasHurtAnim)
            _enemy.PlayAnim("E_Hurt");
        else
        {
            _state.ChangeState(_enemy.IdleState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_enemy.IsAlive) return;

        if (!_hasHurtAnim) return;

        var info = _enemy.anim.GetCurrentAnimatorStateInfo(0);

        if (!info.IsName("E_Hurt"))
        {
            if (_enemy.Target != null)
            {
                float dist = Vector3.Distance(
                    _enemy.transform.position,
                    _enemy.Target.position
                );

                if (dist <= _enemy.dataEnemy.FollowRange)
                    _state.ChangeState(_enemy.MoveState);
                else
                    _state.ChangeState(_enemy.IdleState);
            }
            else
            {
                _state.ChangeState(_enemy.IdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.ResumeMoving();
    }
}
