using UnityEngine;

[CreateAssetMenu(fileName = "MissilerAttackSO", menuName = "EnemySO/MissilerAttackSO")]
public class MissileAttackSO : EnemyAttackSO
{
    [SerializeField] private string _missileKey = "EnemyBulletMissile";
    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        Transform firePoint = enemy.Find("FirePoint");
        if (firePoint == null) return null;

        Vector3 dir = (target.position - firePoint.position).normalized;

        GameObject missile = MultiObjectPool.Instance.SpawnFromPool(_missileKey, firePoint.position, Quaternion.LookRotation(dir));

        if (missile != null && missile.TryGetComponent(out EnemyBulletBase missileScript))
        {
            int enemyDamage = enemy.GetComponent<EnemyStats>().Damage;

            missileScript.Init(dir, enemyDamage);

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
