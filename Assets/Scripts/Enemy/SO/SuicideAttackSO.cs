using System.Collections;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "SuicideAttackSO", menuName = "EnemySO/SuicideAttackSO")]
public class SuicideAttackSO : EnemyAttackSO
{
    [SerializeField] private float _explosionDelay = 2f;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private string _explosionEffectKey = "ExplosionEffect";
    [SerializeField] private float _scaleMultiplier = 1.5f;

    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.StartExlosionAttackCoroutine(ExplodeAfterDelay(enemy, controller));
        }
        return null;
    }

    private IEnumerator ExplodeAfterDelay(Transform enemy, EnemyController controller)
    {
        Vector3 originalScale = enemy.localScale;
        enemy.DOScale(originalScale * _scaleMultiplier, _explosionDelay).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(_explosionDelay);

        EnemyStats stats = enemy.GetComponent<EnemyStats>();
        Collider[] hits = Physics.OverlapSphere(enemy.position, _explosionRadius);
        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(stats.Damage);
            }
        }

        GameObject effect = MultiObjectPool.Instance.SpawnFromPool(_explosionEffectKey, enemy.position, Quaternion.identity);

        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        float totalTime = (particleSystem != null) ? particleSystem.main.duration + particleSystem.main.startLifetime.constantMax : 2f;
        
        controller.StartExlosionAttackCoroutine(ReturnEffectAfter(effect, totalTime));
        
        enemy.localScale = originalScale;
        MultiObjectPool.Instance.ReturnToPool("Enemy", enemy.gameObject);
    }
    private IEnumerator ReturnEffectAfter(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        MultiObjectPool.Instance.ReturnToPool(_explosionEffectKey, effect);
    }
}
