using UnityEngine;

public abstract class PlayerBulletBase : MonoBehaviour
{
    [Header("Attributes: ")]
    [SerializeField] protected int _damage;
    [SerializeField] protected float _speed;
    [SerializeField] protected float lifeTimer = 3f;

    [Header("Effects: ")]
    [SerializeField] protected string _hitEffectKey = "HitEffect";

    protected float _timer;

    [SerializeField] protected BulletType _bulletType;
    public BulletType BulletType => _bulletType;

    protected virtual void OnEnable()
    {
        _timer = 0f;
    }

    protected virtual void Update()
    {
        HandleMovement();
        HandleLifeTime();
    }

    protected abstract void HandleMovement();

    protected void HandleLifeTime()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifeTimer)
        {
            OnLifeTimeExpired();
            ReturnToPool();
        }
    }

    protected virtual void OnLifeTimeExpired() { }

    protected virtual void EffectHitToWall()
    {
        MultiObjectPool.Instance.SpawnFromPool(_hitEffectKey, transform.position, Quaternion.identity);
    }

    protected virtual void ReturnToPool()
    {
        MultiObjectPool.Instance.ReturnToPool(this.gameObject.name, gameObject);
    }

    public virtual void SetDamage(int damage) => this._damage = damage;

    protected abstract void DealDamage(Collider other);

    protected virtual void OnTriggerEnter(Collider other)
    {
        BreakableWall wall = other.GetComponent<BreakableWall>();
        if (wall != null)
        {
            OnHitWall(wall);
            ReturnToPool();
            return;
        }
        DealDamage(other);
    }

    protected virtual void OnHitWall(BreakableWall wall)
    {
    }
}
