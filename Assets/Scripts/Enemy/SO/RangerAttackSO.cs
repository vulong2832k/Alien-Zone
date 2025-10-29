using UnityEngine;

[CreateAssetMenu(fileName = "RangerAttackSO", menuName = "EnemySO/RangerAttackSO")]
public class RangerAttackSO : EnemyAttackSO
{
    [SerializeField] private string _bulletKey = "EnemyBulletNormal";

    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        Transform firePoint = enemy.Find("FirePoint");
        if (firePoint == null) return null;

        Vector3 dir = (target.position - firePoint.position).normalized;

        GameObject bullet = MultiObjectPool.Instance.SpawnFromPool(_bulletKey, firePoint.position, Quaternion.LookRotation(dir));

        if (bullet != null && bullet.TryGetComponent(out EnemyBulletBase bulletScript))
        {
            int enemyDamage = enemy.GetComponent<EnemyStats>().Damage;
            bulletScript.Init(dir, enemyDamage);

            return new AttackResult
            {
                attacker = enemy,
                target = target,
                damage = enemyDamage
            };
        }
        return new AttackResult
        {
            attacker = enemy,
            target = target,
            damage = 0
        };
    }

}
