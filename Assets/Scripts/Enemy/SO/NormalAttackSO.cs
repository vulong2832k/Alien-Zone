using UnityEngine;

[CreateAssetMenu(fileName = "NormalAttackSO", menuName = "EnemySO/NormalAttackSO")]
public class NormalAttackSO : EnemyAttackSO
{
    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        return new AttackResult
        {
            attacker = enemy,
            target = target,
            damage = damage
        };
    }

}
