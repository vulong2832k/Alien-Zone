using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyController enemy, EnemyStateMachine state, SuicideAttackSO attackSO) : base(enemy, state)
    {
    }

    private SuicideAttackSO _attackSO;
    private Vector3 _originalScale;

    public override void Enter()
    {
        base.Enter();
        _enemy.StopMoving();
        _originalScale = _enemy.transform.localScale;
        _enemy.transform.DOScale(_originalScale * _attackSO.ScaleMultiplier, _attackSO.ExplosionDelay);
        _enemy.StartCoroutine(ExplosionCoroutine());
        _enemy.PlayAnim("E_Attack");
    }
    private IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(_attackSO.ExplosionDelay);

        _enemy.DoExplosionDamage(_attackSO.ExplosionRadius, _enemy.stats.Damage);
        _enemy.SpawnExplosionEffect(_attackSO.ExplosionEffectKey);
        _enemy.transform.localScale = _originalScale;
        _enemy.DieFromExplosion();
    }
    public override void Exit()
    {
        base.Exit();
        DOTween.Kill(_enemy.transform);
        _enemy.transform.localScale = _originalScale;
    }
}
