using UnityEngine;

public class EnemyDeathState : EnemyState
{
    private float _timeCounter = 0f;

    public EnemyDeathState(EnemyController enemy, EnemyStateMachine state)
        : base(enemy, state) { }

    public override void Enter()
    {
        base.Enter();

        this._timeCounter = 0f;

        _enemy.StopMoving();

        _enemy.PlayAnim("E_Death");

        var col = _enemy.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (_enemy.Agent != null)
            _enemy.Agent.enabled = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        this._timeCounter += Time.deltaTime;

        if (this._timeCounter < 0.1f) return;

        var animState = _enemy.anim.GetCurrentAnimatorStateInfo(0);

        if (animState.IsName("E_Death") && animState.normalizedTime >= 1f)
        {
            _enemy.OnDeathComplete();
        }
    }
}
