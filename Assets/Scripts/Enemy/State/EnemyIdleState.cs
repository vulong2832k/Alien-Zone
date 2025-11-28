using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController enemy, EnemyStateMachine state) : base(enemy, state)
    {
    }

    public override void LogicUpdate()
    {
        if (!_enemy.IsAlive) return;

        float dist = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);
        if (dist <= _enemy.dataEnemy.AttackRange)
        {
            //_state.ChangeState(_enemy.AttackState);
        }
        else
        {
            //_state.ChangeState(_enemy.ChaseState);
        }
    }
}
