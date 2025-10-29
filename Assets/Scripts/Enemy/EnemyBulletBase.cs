using UnityEngine;

public abstract class EnemyBulletBase : MonoBehaviour
{
    [SerializeField] protected BulletEnemySO _data;

    protected float _lifeTimer;
    protected Vector3 _direction;

    private int _currentDamage;

    protected virtual void OnEnable()
    {
        _lifeTimer = _data.lifeTime;
    }

    protected virtual void Update()
    {
        HandleMovement();

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            OnLifeTimeExpried();
        }
    }

    protected abstract void HandleMovement();

    protected virtual void OnLifeTimeExpried()
    {
        Explode();
        MultiObjectPool.Instance.ReturnToPool(_data.poolKey, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.isTrigger) return;

        if (!_data.explosive && other.CompareTag("Player") && other.transform.root.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_currentDamage);
        }
        Explode();
        MultiObjectPool.Instance.ReturnToPool(_data.poolKey, gameObject);
    }
    protected virtual void Explode()
    {
        int baseDamage = _data.explosionDamage > 0 ? _data.explosionDamage : _currentDamage;

        Collider[] hits = Physics.OverlapSphere(transform.position, _data.explosionRadius);
        foreach (var hit in hits)
        {
            IDamageable target = null;

            if (hit.TryGetComponent(out IDamageable comp) || hit.transform.root.TryGetComponent(out comp))
            {
                target = comp;
            }

            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);

                if (distance > _data.explosionRadius) continue;
                float damageMultiplier = Mathf.Max(0f, 1f - 0.2f * Mathf.Floor(distance));
                int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);

                if (finalDamage > 0)
                    target.TakeDamage(finalDamage);
            }
        }

        GameObject explosionVFX = MultiObjectPool.Instance.SpawnFromPool("EffectExplosionMissile", transform.position, Quaternion.identity);
        if (explosionVFX != null)
        {
            var ps = explosionVFX.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosionVFX, ps.main.duration);
            }
        }
    }
    public virtual void Init(Vector3 direction, int damage)
    {
        _direction = direction.normalized;
        _currentDamage = damage;
    }
}
