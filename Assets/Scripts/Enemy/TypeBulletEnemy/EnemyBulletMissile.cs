using UnityEngine;

public class EnemyBulletMissile : EnemyBulletBase
{
    [SerializeField] private float _rotateSpeed;
    private Transform _target;

    protected override void OnEnable()
    {
        base.OnEnable();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _target = player != null ? player.transform : null;
    }

    protected override void HandleMovement()
    {
        if (_target == null)
        {
            transform.position += _direction * _data.speed * Time.deltaTime;
            return;
        }

        Vector3 dirToTarget = (_target.position - transform.position).normalized;
        _direction = Vector3.Slerp(_direction, dirToTarget, _rotateSpeed * Time.deltaTime).normalized;
        transform.position += _direction * _data.speed * Time.deltaTime;

        if (_direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
}
