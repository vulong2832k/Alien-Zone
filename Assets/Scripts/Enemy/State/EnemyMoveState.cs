using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(EnemyController enemy, EnemyStateMachine state) : base(enemy, state)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.ResumeMoving();
        _enemy.PlayAnim("E_Run");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_enemy.IsAlive) return;
        if (_enemy.Target == null) return;

        float dist = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);

        if (dist <= _enemy.dataEnemy.AttackRange)
        {
            _enemy.StopMoving();
            _state.ChangeState(_enemy.AttackState);
            return;
        }

        if (dist > _enemy.dataEnemy.FollowRange)
        {
            _state.ChangeState(_enemy.IdleState);
            return;
        }

        _enemy.Agent.SetDestination(_enemy.Target.position);

        Vector3 dir = _enemy.Agent.velocity.normalized;
        if (dir != Vector3.zero)
            _enemy.transform.rotation = Quaternion.LookRotation(dir);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
