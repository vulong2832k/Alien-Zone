using UnityEngine;

public class AttackResult
{
    public int damage;
    public Transform attacker;
    public Transform target;
}

public abstract class EnemyAttackSO : ScriptableObject
{
    public abstract AttackResult EnemyAttack(Transform enemy, Transform target, int damage);
}
