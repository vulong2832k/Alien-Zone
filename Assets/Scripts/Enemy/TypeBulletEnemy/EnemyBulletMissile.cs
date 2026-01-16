using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBulletMissile : EnemyBulletBase
{
    [SerializeField] private float _rotateSpeed = 5f;

    private Transform _target;
    private Rigidbody _rb;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.linearVelocity = Vector3.zero;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _target = player != null ? player.transform : null;
    }

    public override void Init(Vector3 direction, int damage)
    {
        base.Init(direction, damage);

        if (_rb == null)
            _rb = GetComponent<Rigidbody>();

        _rb.linearVelocity = _direction * _data.speed;
        RotateToDirection(_direction);
    }

    protected override void Update()
    {
        base.Update();

        if (_target == null)
            return;

        Vector3 dirToTarget = (_target.position - transform.position).normalized;

        _direction = Vector3.Slerp(
            _direction,
            dirToTarget,
            _rotateSpeed * Time.deltaTime
        ).normalized;

        _rb.linearVelocity = _direction * _data.speed;

        RotateToDirection(_direction);
    }

    private void RotateToDirection(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
