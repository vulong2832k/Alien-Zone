using UnityEngine;

public class EnemyBulletNormal : EnemyBulletBase
{
    protected override void HandleMovement()
    {
        transform.position += _direction * _data.speed * Time.deltaTime;
    }
}
