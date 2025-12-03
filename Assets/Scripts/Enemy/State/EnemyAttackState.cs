using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private EnemyAttackSO _attackSO;
    private Vector3 _originalScale;

    private float _attackTimer;

    public EnemyAttackState(EnemyController enemy, EnemyStateMachine state, EnemyAttackSO attackSO)
        : base(enemy, state)
    {
        _attackSO = attackSO;
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.StopMoving();
        _enemy.PlayAnim("E_Attack");

        _attackTimer = _attackSO.AttackDuration;

        if (_attackSO is SuicideAttackSO suicideSO)
        {
            _originalScale = _enemy.transform.localScale;
            _enemy.transform.DOScale(_originalScale * suicideSO.ScaleMultiplier, suicideSO.ExplosionDelay);
            _enemy.StartCoroutine(ExplosionCoroutine(suicideSO));
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!(_attackSO is SuicideAttackSO))
        {
            _enemy.FaceTarget();
        }

        if (_attackSO is SuicideAttackSO)
            return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            _state.ChangeState(_enemy.IdleState);
        }
    }

    private IEnumerator ExplosionCoroutine(SuicideAttackSO suicideSO)
    {
        yield return new WaitForSeconds(suicideSO.ExplosionDelay);

        _enemy.DoExplosionDamage(suicideSO.ExplosionRadius, _enemy.stats.Damage);
        _enemy.SpawnExplosionEffect(suicideSO.ExplosionEffectKey);
        _enemy.transform.localScale = _originalScale;
        _enemy.DieFromExplosion();
    }

    public override void Exit()
    {
        base.Exit();

        DOTween.Kill(_enemy.transform);

        if (_attackSO is SuicideAttackSO)
            _enemy.transform.localScale = _originalScale;
    }
}
