using UnityEngine;

public class DamageBox : MonoBehaviour
{
    private int _damage;
    private EnemyController _owner;

    public void Init(EnemyController owner, int damage, float lifeTime)
    {
        _owner = owner;
        _damage = damage;
        gameObject.SetActive(true);
        Invoke(nameof(Disable), lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null && target != _owner)
            target.TakeDamage(_damage);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
