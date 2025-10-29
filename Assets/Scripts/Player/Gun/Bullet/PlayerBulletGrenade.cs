using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBulletGrenade : PlayerBulletBase
{
    [SerializeField] private float _explosionRadius = 5f;
    private Rigidbody _rb;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    protected override void HandleMovement()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    protected override void DealDamage(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Explode();
        }
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
        wall.TakeDamage(3);
    }
}
