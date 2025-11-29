using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController enemy, EnemyStateMachine state) : base(enemy, state)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemy.StopMoving();

        _enemy.PlayAnim("E_Idle");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!_enemy.IsAlive) return;
        if (_enemy.Target == null) return;

        float dist = Vector3.Distance(_enemy.transform.position, _enemy.Target.position);

        if (dist <= _enemy.dataEnemy.FollowRange)
        {
            _state.ChangeState(_enemy.MoveState);
            return;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
