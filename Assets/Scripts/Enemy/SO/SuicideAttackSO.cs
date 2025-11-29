using UnityEngine;

[CreateAssetMenu(fileName = "SuicideAttackSO", menuName = "EnemySO/SuicideAttackSO")]
public class SuicideAttackSO : EnemyAttackSO
{
    public float ExplosionDelay = 2f;
    public float ExplosionRadius = 3f;
    public float ScaleMultiplier = 1.5f;
    public string ExplosionEffectKey = "ExplosionEffect";
    public override AttackResult EnemyAttack(Transform enemy, Transform target, int damage)
    {
        return new AttackResult
        {
            damage = damage,
            target = target
        };
    }
}
