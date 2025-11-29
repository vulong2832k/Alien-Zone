using UnityEngine;

[CreateAssetMenu(fileName = "MissilerAttackSO", menuName = "EnemySO/MissilerAttackSO")]
public class MissileAttackSO : EnemyAttackSO
{
    public string MissileKey = "EnemyBulletMissile";

    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        return new AttackResult
        {
            attacker = enemy,
            target = target,
            damage = damage,
            extraString = MissileKey,
        };
    }
}
