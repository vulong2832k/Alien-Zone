using UnityEngine;

[CreateAssetMenu(fileName = "RangerAttackSO", menuName = "EnemySO/RangerAttackSO")]
public class RangerAttackSO : EnemyAttackSO
{
    public string BulletKey = "EnemyBulletNormal";

    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        return new AttackResult
        {
            attacker = enemy,
            target = target,
            damage = damage,
            extraString = BulletKey,
        };
    }

}
