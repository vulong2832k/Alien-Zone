using UnityEngine;

public class PlayerBulletMissile : PlayerBulletBase
{
    [SerializeField] private float _rotateSpeed = 5f;
    [SerializeField] private float _explosionRadius = 5f;
    private Transform _target;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        _target = enemy != null ? enemy.transform : null;
    }

    protected override void HandleMovement()
    {
        if (_target == null)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            return;
        }

        Vector3 dirToTarget = (_target.position - transform.position).normalized;
        Vector3 newDir = Vector3.Slerp(transform.forward, dirToTarget, _rotateSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDir);
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    protected override void DealDamage(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Explode();
        }
    }

    protected override void OnLifeTimeExpired()
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
                dmg.TakeDamage(_damage);
        }

        EffectHitToWall();
        ReturnToPool();
    }
    protected override void OnHitWall(BreakableWall wall)
    {
        wall.TakeDamage(9999);
    }
}
