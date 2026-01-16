using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBulletNormal : EnemyBulletBase
{
    private Rigidbody _rb;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.linearVelocity = Vector3.zero;
    }

    public override void Init(Vector3 direction, int damage)
    {
        base.Init(direction, damage);

        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.linearVelocity = _direction * _data.speed;
    }
}
