using UnityEngine;

public class PlayerBulletNormal : PlayerBulletBase
{
    protected override void HandleMovement()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    protected override void DealDamage(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            ReturnToPool();
            return;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            EffectHitToWall();
            ReturnToPool();
        }
    }
    protected override void OnHitWall(BreakableWall wall)
    {
        wall.TakeDamage(1);
    }
}
